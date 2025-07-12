<template>
  <div>
    <div class="top-data">
      <div class="my-field">
        <h3>
          Всего вложено:
          <span v-if="isSuccessSeedMoney">
            {{ isHide ? 0 : formatCurrency(seedMoney!, { maxFractionDigits: 0 }) }}
          </span>
        </h3>
        <h3>
          Всего куплено:
          <span v-if="isSuccessTotalMoney">
            {{ isHide ? 0 : formatCurrency(totalMoney!, { maxFractionDigits: 0 }) }}
          </span>
        </h3>
        <h3 v-if="isLive">
          Всего на сейчас:
          <span v-if="isSuccessTotalMoneyNow">
            {{ isHide ? 0 : formatCurrency(totalMoneyNow!, { maxFractionDigits: 0 }) }}
          </span>
        </h3>
        <div class="currency-table">
          <DataTable :value="avgPrices">
            <Column field="currency" header="currency">
              <template #body="{ data }">
                {{ isHide ? '*' : data.currency }}
              </template>
            </Column>
            <Column field="avgPrice" header="avgPrice">
              <template #body="{ data }">
                {{ isHide ? '*' : formatCurrency(data.avgPrice, { currency: data.currency }) }}
              </template>
            </Column>
            <Column v-if="isLive && !isHide" field="avgPriceNow" header="avgPriceNow">
              <template #body="{ data }">
                {{ isHide ? '*' : formatCurrency(data.avgPriceNow, { currency: data.currency }) }}
              </template>
            </Column>
          </DataTable>
        </div>
      </div>
      <div class="ministat-table">
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
                :alwaysShowPaginator="false">
                <Column field="country" header="country">
                  <template #body="{ data }">
                    {{ isHide ? '*' : data.country }}
                  </template>
                </Column>
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
                    {{ isHide ? '*' : data.percent + '%' }}
                  </template>
                </Column>
                <Column v-if="isLive && !isHide" header="sumRubNow">
                  <template #body="{ data }">
                    {{ isHide ? '*' : formatCurrency(data.sumRubNow, { maxFractionDigits: 0 }) }}
                  </template>
                </Column>
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
                :alwaysShowPaginator="false">
                <Column field="sector" header="sector">
                  <template #body="{ data }">
                    {{ isHide ? '*' : data.sector }}
                  </template>
                </Column>
                <Column field="sumRub" header="sumRub" sortable>
                  <template #body="{ data }">
                    {{ isHide ? '*' : formatCurrency(data.sumRub, { maxFractionDigits: 0 }) }}
                  </template>
                </Column>
                <Column field="percent" header="percent">
                  <template #body="{ data }">
                    {{ isHide ? '*' : data.percent + '%' }}
                  </template>
                </Column>
                <Column v-if="isLive && !isHide" header="sumRubNow">
                  <template #body="{ data }">
                    {{ isHide ? '*' : formatCurrency(data.sumRubNow, { maxFractionDigits: 0 }) }}
                  </template>
                </Column>
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
                :alwaysShowPaginator="false">
                <Column field="type" header="type">
                  <template #body="{ data }">
                    {{ isHide ? '*' : data.type }}
                  </template>
                </Column>
                <Column field="sumRub" header="sumRub" sortable>
                  <template #body="{ data }">
                    {{ isHide ? '*' : formatCurrency(data.sumRub, { maxFractionDigits: 0 }) }}
                  </template>
                </Column>
                <Column field="percent" header="percent">
                  <template #body="{ data }">
                    {{ isHide ? '*' : data.percent + '%' }}
                  </template>
                </Column>
                <Column v-if="isLive && !isHide" header="sumRubNow">
                  <template #body="{ data }">
                    {{ isHide ? '*' : formatCurrency(data.sumRubNow, { maxFractionDigits: 0 }) }}
                  </template>
                </Column>
              </DataTable>
            </TabPanel>
          </TabPanels>
        </Tabs>
      </div>
    </div>
    <ToggleButton :onIcon="PrimeIcons.EYE" :offIcon="PrimeIcons.EYE_SLASH" v-model="isHide" onLabel="" offLabel="" />
    <ToggleButton
      :onIcon="PrimeIcons.STOP"
      :offIcon="PrimeIcons.SYNC"
      v-model="isLive"
      onLabel=""
      offLabel=""
      @update:model-value="onSync" />
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
      @rowSelect="onRowSelect"
      selectionMode="single">
      <template #header>
        <div class="">
          <Button icon="pi pi-filter-slash" label="Clear" outlined type="button" @click="clearFilter()" />
        </div>
      </template>
      <template #empty>No data found.</template>

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
      <Column field="percent" header="percent" dataType="numeric" sortable>
        <template #body="{ data }">
          {{ isHide ? '*' : data.percent + '%' }}
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
      <Column field="country" header="Country" :showFilterMatchModes="false" sortable>
        <template #filter="{ filterModel }">
          <MultiSelect v-model="filterModel.value" :options="countryStat?.map((x) => x.country)" placeholder="Any" />
        </template>
      </Column>
      <Column field="sector" header="Sector" :showFilterMatchModes="false" sortable>
        <template #filter="{ filterModel }">
          <MultiSelect v-model="filterModel.value" :options="sectorStat?.map((x) => x.sector)" placeholder="Any" />
        </template>
      </Column>
      <Column field="type" header="Type" :showFilterMatchModes="false" sortable>
        <template #filter="{ filterModel }">
          <MultiSelect v-model="filterModel.value" :options="typeStat?.map((x) => x.type)" placeholder="Any" />
        </template>
      </Column>
      <Column v-if="isLive && !isHide" field="sumRubNow" header="sumRubNow">
        <template #body="{ data }">
          {{ isHide ? '*' : formatCurrency(data.sumRubNow, { maxFractionDigits: 0 }) }}
        </template>
      </Column>
      <Column v-if="isLive && !isHide" field="avgNow" header="avgNow">
        <template #body="{ data }">
          {{ isHide ? '*' : formatCurrency(data.sumRubNow / data.sumCount, { maxFractionDigits: 0 }) }}
        </template>
      </Column>
    </DataTable>
    <!-- <TD v-if="isLive" :search="search"></TD> -->
    <div style="font-size: xx-large; display: flex; justify-content: center; padding-top: 50px">
      <a
        target="_blank"
        rel="noopener noreferrer"
        :href="`https://www.tradingview.com/chart/?symbol=${search}`"
        style="">
        ссылка на tradingview
      </a>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { useAvgPrices } from '@/shared/api/CurrencyApi';
import {
  useStatistics,
  useStatisticsByCountry,
  useStatisticsBySector,
  useStatisticsByType,
  useTotalMoney,
} from '@/shared/api/StatisticsApi';
import { useSeedMoney } from '@/shared/api/TradesApi.ts';
import type { Bookmark, Statistics } from '@/shared/generated';
import { useCurrencyOptions } from '@/shared/utils/enums';
import { formatCurrency } from '@/shared/utils/num';
import { useLocalStorage } from '@/shared/utils/useLocalStorage';
import { FilterMatchMode, FilterOperator, PrimeIcons } from '@primevue/core/api';
import type { DataTableFilterMeta, DataTableRowSelectEvent } from 'primevue/datatable';
import { computed, ref } from 'vue';

const search = ref('RUS:SIBN');
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
    country: { value: null, matchMode: FilterMatchMode.IN },
    sector: { value: null, matchMode: FilterMatchMode.IN },
    type: { value: null, matchMode: FilterMatchMode.IN },
  };
  pageSize.value = 10;
  page.value = 0;
};
const clearFilter = () => {
  initFilters();
};

initFilters();
const { data: seedMoney, isSuccess: isSuccessSeedMoney } = useSeedMoney();
const { data: totalMoney, isSuccess: isSuccessTotalMoney } = useTotalMoney(false);
const { data: totalMoneyNow, isSuccess: isSuccessTotalMoneyNow } = useTotalMoney(isLive);

const selected = ref();
const { data } = useStatistics(isLive);
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
        copy.country = '';
        copy.sector = '';
        copy.type = '';
        return copy;
      }),
);
const template = computed(() => (!isHide.value ? '{first} to {last} of {totalRecords}' : '0'));
const { data: countryStat } = useStatisticsByCountry(isLive);
const { data: sectorStat } = useStatisticsBySector(isLive);
const { data: typeStat } = useStatisticsByType(isLive);
const { data: avgPrices } = useAvgPrices(isLive);
function onSync(bool: boolean) {
  if (bool) {
    // TODO get online prices
  }
}
function onRowSelect(event: DataTableRowSelectEvent<Statistics>) {
  search.value = event.data.source + ':' + event.data.ticker;
}
</script>

<style scoped>
.my-field {
  width: 500px;
  padding-bottom: 40px;
  padding-right: 100px;
  color: hsla(160, 100%, 37%, 1);
}

.top-data {
  display: flex;
}

.ministat-table {
  font-size: large;
}

.currency-table {
  padding-top: 15px;
}
</style>
