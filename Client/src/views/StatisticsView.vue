<template>
  <div>
    <div class="up-data">
      <div class="my-field">
        <h3>
          Всего вложено:
          <span v-if="isSuccess">{{ isHide ? 0 : formatCurrency(seedMoney!, { maxFractionDigits: 0 }) }}</span>
        </h3>
        <h3>
          Всего куплено: <span>{{ isHide ? 0 : formatCurrency(totalBought, { maxFractionDigits: 0 }) }}</span>
        </h3>
      </div>
      <div class="mini-table">
        <Tabs value="0">
          <TabList>
            <Tab value="0">По странам</Tab>
            <Tab value="1">По секторам</Tab>
            <Tab value="2">По типам активов</Tab>
          </TabList>
          <TabPanels>
            <TabPanel value="0">
              <DataTable
                :value="countryStat"
                sort-field="sumRub"
                :sort-order="-1"
                removable-sort
                paginator
                :rows="5"
                :alwaysShowPaginator="false"
              >
                <Column field="country" header="country"></Column>
                <Column field="sum" header="sum">
                  <template #body="{ data }">
                    {{ isHide ? '*' : formatCurrency(data.sum, { currency: data.currency, maxFractionDigits: 0 }) }}
                  </template>
                </Column>
                <Column field="sumRub" header="sumRub" sortable>
                  <template #body="{ data }">
                    {{ isHide ? '*' : formatCurrency(data.sumRub, { maxFractionDigits: 0 }) }}
                  </template>
                </Column>
                <Column field="percent" header="percent">
                  <template #body="{ data }">
                    {{ isHide ? '*' : ((data.sumRub / totalBought) * 100).toFixed(2) + '%' }}
                  </template>
                </Column>
                <Column v-if="isLive && !isHide" header="now"> <template #body="{ data }"> now </template></Column>
              </DataTable>
            </TabPanel>
            <TabPanel value="1">
              <DataTable
                :value="sectorStat"
                sort-field="sumRub"
                :sort-order="-1"
                removable-sort
                paginator
                :rows="5"
                :alwaysShowPaginator="false"
              >
                <Column field="sector" header="sector"></Column>
                <Column field="sumRub" header="sumRub" sortable>
                  <template #body="{ data }">
                    {{ isHide ? '*' : formatCurrency(data.sumRub, { maxFractionDigits: 0 }) }}
                  </template>
                </Column>
                <Column field="percent" header="percent">
                  <template #body="{ data }">
                    {{ isHide ? '*' : ((data.sumRub / totalBought) * 100).toFixed(2) + '%' }}
                  </template>
                </Column>
                <Column v-if="isLive && !isHide" header="now"> <template #body="{ data }"> now </template></Column>
              </DataTable>
            </TabPanel>
            <TabPanel value="2">
              <DataTable
                :value="typeStat"
                sort-field="sumRub"
                :sort-order="-1"
                removable-sort
                paginator
                :rows="5"
                :alwaysShowPaginator="false"
              >
                <Column field="type" header="type"></Column>
                <Column field="sumRub" header="sumRub" sortable>
                  <template #body="{ data }">
                    {{ isHide ? '*' : formatCurrency(data.sumRub, { maxFractionDigits: 0 }) }}
                  </template>
                </Column>
                <Column field="percent" header="percent">
                  <template #body="{ data }">
                    {{ isHide ? '*' : ((data.sumRub / totalBought) * 100).toFixed(2) + '%' }}
                  </template>
                </Column>
                <Column v-if="isLive && !isHide" header="now"> <template #body="{ data }"> now </template></Column>
              </DataTable>
            </TabPanel>
          </TabPanels>
        </Tabs>
      </div>
    </div>
    <ToggleButton :onIcon="PrimeIcons.EYE" :offIcon="PrimeIcons.EYE_SLASH" v-model="isHide" onLabel="" offLabel="" />
    <ToggleButton :onIcon="PrimeIcons.STOP" :offIcon="PrimeIcons.SYNC" v-model="isLive" onLabel="" offLabel="" />
    <DataTable
      v-model:filters="filters"
      v-model:selection="selected"
      v-model:rows="pageSize"
      v-model:first="page"
      :rowsPerPageOptions="[5, 10, 20, 50]"
      paginatorTemplate="FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink RowsPerPageDropdown"
      v-model:currentPageReportTemplate="template"
      :value="statistics"
      dataKey="id"
      filterDisplay="menu"
      paginator
      removableSort
      stripedRows
    >
      <template #header>
        <div class="">
          <Button icon="pi pi-filter-slash" label="Clear" outlined type="button" @click="clearFilter()" />
        </div>
      </template>
      <template #empty> No data found.</template>
      <Column headerStyle="width: 3rem" selectionMode="multiple"></Column>

      <Column field="ticker" header="Ticker" sortable>
        <template #filter="{ filterModel }">
          <InputText v-model="filterModel.value" placeholder="Search by Ticker" type="text" />
        </template>
      </Column>
      <Column field="avgPrice" header="AvgPrice" dataType="numeric" sortable>
        <template #body="{ data }">
          {{ isHide ? '*' : formatCurrency(data.avgPrice, { currency: data.currency }) }}
        </template>
        <template #filter="{ filterModel }">
          <InputNumber v-model="filterModel.value" currency="RUB" locale="ru-RU" mode="currency" />
        </template>
      </Column>
      <Column field="sumCount" header="sumCount" dataType="numeric" sortable>
        <template #filter="{ filterModel }">
          <InputNumber v-model="filterModel.value" mode="decimal" />
        </template>
      </Column>
      <Column field="total" header="total" dataType="numeric" sortable>
        <template #body="{ data }">
          {{ isHide ? '*' : formatCurrency(data.total, { currency: data.currency }) }}
        </template>
        <template #filter="{ filterModel }">
          <InputNumber v-model="filterModel.value" currency="RUB" locale="ru-RU" mode="currency" />
        </template>
      </Column>
      <Column field="currency" header="Currency" :showFilterMatchModes="false" sortable>
        <template #filter="{ filterModel }">
          <MultiSelect v-model="filterModel.value" :options="useCurrencyOptions()" placeholder="Any" />
        </template>
      </Column>
      <Column v-if="isLive && !isHide" header="now">
        <template #body="{ data }">
          <a :href="`https://www.tradingview.com/chart/?symbol=${bookDict.get(data.ticker)!.source}:${data.ticker}`">
            ссылка на tradingview
          </a>
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<script lang="ts" setup>
import { useBookmarks } from '@/shared/api/BookmarksApi';
import { useStatistics } from '@/shared/api/StatisticsApi';
import { useSeedMoney } from '@/shared/api/TradesApi.ts';
import type { Bookmark, Currency } from '@/shared/generated';
import { useCurrencyOptions } from '@/shared/utils/enums';
import { formatCurrency } from '@/shared/utils/num';
import { useLocalStorage } from '@/shared/utils/useLocalStorage';
import { FilterMatchMode, FilterOperator, PrimeIcons } from '@primevue/core/api';
import type { DataTableFilterMeta } from 'primevue/datatable';
import { computed, ref } from 'vue';

const isLive = ref(false);
const pageSize = ref(10);
const page = ref(0);
const isHide = useLocalStorage('isHide', false);
const filters = ref<DataTableFilterMeta>();
const initFilters = () => {
  filters.value = {
    ticker: {
      operator: FilterOperator.OR,
      constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }],
    },
    avgPrice: {
      operator: FilterOperator.OR,
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
    },
    sumCount: {
      operator: FilterOperator.OR,
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
    },
    total: {
      operator: FilterOperator.OR,
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
    },
    currency: { value: null, matchMode: FilterMatchMode.IN },
  };
  pageSize.value = 10;
  page.value = 0;
};
const clearFilter = () => {
  initFilters();
};

initFilters();
const { data: seedMoney, isSuccess } = useSeedMoney();
const selected = ref();
const { data } = useStatistics();
const statistics = computed(() =>
  !isHide.value
    ? data.value
    : data.value?.map((x) => {
        const copy = structuredClone({ ...x });
        copy.avgPrice = -1;
        copy.sumCount = -1;
        copy.ticker = '*';
        copy.total = -1;
        copy.currency = 'RUB';
        return copy;
      }),
);
const template = computed(() => (!isHide.value ? '{first} to {last} of {totalRecords}' : '0'));
const totalBought = computed(
  () => statistics.value?.reduce((sum, next) => sum + IntoRub(next.total, next.currency), 0) ?? 1,
);
const { data: bookmarks } = useBookmarks();
const bookDict = computed(() => {
  const map = new Map<string, Bookmark>();
  return (
    bookmarks.value?.reduce((map, next) => {
      map.set(next.ticker!, next);
      return map;
    }, map) ?? map
  );
});
const countryStat = computed(() =>
  !isHide.value
    ? statistics.value?.reduce(
        (arr, next) => {
          const country = bookDict.value.get(next.ticker)!.country!;
          if (!arr.some((x) => x.country == country)) {
            arr.push({
              country: country,
              sum: next.total,
              sumRub: IntoRub(next.total, next.currency),
              currency: next.currency,
            });
          } else {
            const first = arr.filter((x) => x.country == country)[0];
            first.sum += next.total;
            first.sumRub += IntoRub(next.total, next.currency);
          }
          return arr;
        },
        [] as { country: string; sum: number; sumRub: number; currency: Currency }[],
      )
    : [{ country: '*', sum: 0 }],
);
const sectorStat = computed(() =>
  !isHide.value
    ? statistics.value?.reduce(
        (arr, next) => {
          const sector = bookDict.value.get(next.ticker)!.sector!;
          if (!arr.some((x) => x.sector == sector)) {
            arr.push({
              sector: sector,
              sumRub: IntoRub(next.total, next.currency),
              currency: next.currency,
            });
          } else {
            const first = arr.filter((x) => x.sector == sector)[0];
            first.sumRub += IntoRub(next.total, next.currency);
          }
          return arr;
        },
        [] as { sector: string; sumRub: number; currency: Currency }[],
      )
    : [{ sector: '*', sumRub: 0 }],
);
const typeStat = computed(() =>
  !isHide.value
    ? statistics.value?.reduce(
        (arr, next) => {
          const type = bookDict.value.get(next.ticker)!.type!;
          if (!arr.some((x) => x.type == type)) {
            arr.push({
              type: type,
              sumRub: IntoRub(next.total, next.currency),
              currency: next.currency,
            });
          } else {
            const first = arr.filter((x) => x.type == type)[0];
            first.sumRub += IntoRub(next.total, next.currency);
          }
          return arr;
        },
        [] as { type: string; sumRub: number; currency: Currency }[],
      )
    : [{ type: '*', sumRub: 0 }],
);
function IntoRub(value: number, currency: Currency) {
  let total: number;
  switch (currency) {
    case 'RUB':
      total = value;
      break;
    case 'USD':
      total = value * 74;
      break;
    case 'EUR':
      total = value * 94;
      break;
  }
  return total;
}
</script>

<style scoped>
.my-field {
  width: 400px;
  padding-bottom: 40px;
  padding-right: 100px;
  color: hsla(160, 100%, 37%, 1);
}
.up-data {
  display: flex;
}
.mini-table {
  font-size: large;
}
</style>
