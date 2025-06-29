<template>
  <div>
    <div class="up-data">
      <div class="my-field">
        <h3>
          Всего вложено: <span v-if="isSuccess">{{ isHide ? 0 : formatCurrency(seedMoney!, 0) }}</span>
        </h3>
        <h3>
          Всего куплено: <span>{{ isHide ? 0 : formatCurrency(totalBuyed) }}</span>
        </h3>
      </div>
      <div class="mini-table">
        <DataTable :value="resultSum" removable-sort>
          <Column field="country" header="country" sortable></Column>
          <Column field="sum" header="sum" sortable>
            <template #body="{ data }">
              {{ isHide ? '*' : formatCurrency(data.sum) }}
            </template>
          </Column>
        </DataTable>
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
          {{ isHide ? '*' : formatCurrency(data.avgPrice) }}
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
          {{ isHide ? '*' : formatCurrency(data.total) }}
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
      <Column v-if="isLive" header="now">
        <template #body="{ data }">
          <a :href="`https://www.tradingview.com/chart/?symbol=${data.source}:${data.ticker}`">ссылка на tradingview</a>
        </template></Column
      >
    </DataTable>
  </div>
</template>

<script lang="ts" setup>
import { useStatistics } from '@/shared/api/StatisticsApi';
import { useSeedMoney } from '@/shared/api/TradesApi.ts';
import type { Currency } from '@/shared/generated';
import { useCurrencyOptions } from '@/shared/utils/enums';
import { formatCurrency } from '@/shared/utils/num';
import { FilterMatchMode, FilterOperator, PrimeIcons } from '@primevue/core/api';
import type { DataTableFilterMeta } from 'primevue/datatable';
import { computed, ref, shallowReactive } from 'vue';

const isLive = ref(false);
const pageSize = ref(10);
const page = ref(0);
const isHide = ref(false);
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
        copy.currency = undefined;
        return copy;
      }),
);
const template = computed(() => (!isHide.value ? '{first} to {last} of {totalRecords}' : '1'));
const totalBuyed = computed(() => statistics.value?.reduce((sum, next) => sum + next.total!, 0) ?? 0);
const resultSum = computed(() =>
  statistics.value?.reduce(
    (arr, next) => {
      if (!arr.some((x) => x.country == next.currency)) {
        arr.push({ country: next.currency, sum: next.total });
      } else {
        const first = arr.filter((x) => x.country == next.currency)[0];
        first.sum += next.total;
      }
      return arr;
    },
    [] as { country: Currency; sum: number }[],
  ),
);
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
