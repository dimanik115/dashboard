<template>
  <div :class="{ 'all-hide': isToggled }">
    <ToggleButton v-model="isToggled" onLabel="Раскрыть" offLabel="Скрыть" />
    <div class="my-field">
      <h3>
        Вложено: <span :class="{ 'my-hide': isToggled }" v-if="isSuccess">{{ formatCurrency(seedMoney!, 0) }}</span>
      </h3>
    </div>
    <DataTable
      v-model:filters="filters"
      v-model:selection="selected"
      :rows="10"
      :rowsPerPageOptions="[5, 10, 20, 50]"
      :value="trades"
      dataKey="id"
      filterDisplay="menu"
      paginator
      removableSort
      sortMode="multiple"
      stripedRows
      tableStyle="min-width: 50rem"
    >
      <template #header>
        <div class="">
          <Button icon="pi pi-filter-slash" label="Clear" outlined type="button" @click="clearFilter()" />
        </div>
      </template>
      <template #empty> No trades found.</template>
      <Column headerStyle="width: 3rem" selectionMode="multiple"></Column>

      <Column field="ticker" header="Ticker" sortable>
        <template #filter="{ filterModel }">
          <InputText v-model="filterModel.value" placeholder="Search by Ticker" type="text" />
        </template>
      </Column>
      <Column field="tradeDate" header="TradeDate" dataType="date" sortable>
        <template #body="{ data }">
          {{ formatDate(data.tradeDate) }}
        </template>
        <template #filter="{ filterModel }">
          <DatePicker
            :value="filterModel.value"
            @update:model-value="(v) => (filterModel.value = v)"
            dateFormat="dd.mm.yy"
            placeholder="dd.mm.yyyy"
            showButtonBar
            @clear-click="filterModel.value = null"
          />
        </template>
      </Column>
      <Column field="avgPrice" header="AvgPrice" dataType="numeric" sortable>
        <template #body="{ data }">
          {{ formatCurrency(data.avgPrice) }}
        </template>
        <template #filter="{ filterModel }">
          <InputNumber v-model="filterModel.value" currency="RUB" locale="ru-RU" mode="currency" />
        </template>
      </Column>
      <Column field="count" header="Count" dataType="numeric" sortable>
        <template #filter="{ filterModel }">
          <InputNumber v-model="filterModel.value" mode="decimal" />
        </template>
      </Column>
      <Column field="sum" header="Sum" dataType="numeric" sortable>
        <template #body="{ data }">
          {{ formatCurrency(data.sum) }}
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
      <Column field="buySell" header="BuySell" :showFilterMatchModes="false" sortable>
        <template #filter="{ filterModel }">
          <MultiSelect v-model="filterModel.value" :options="useBuySellOptions()" placeholder="Any" />
        </template>
      </Column>
      <Column field="broker" header="Broker" :showFilterMatchModes="false" sortable>
        <template #filter="{ filterModel }">
          <MultiSelect v-model="filterModel.value" :options="useBrokerOptions()" placeholder="Any" />
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<script lang="ts" setup>
import { computed, ref } from 'vue';
import { FilterMatchMode, FilterOperator } from '@primevue/core/api';
import type { DataTableFilterMeta } from 'primevue/datatable';
import { useSeedMoney, useTrades } from '@/shared/api/TradesApi.ts';
import { formatDate } from '@/shared/utils/date';
import { formatCurrency } from '@/shared/utils/num';
import { useBrokerOptions, useBuySellOptions, useCurrencyOptions } from '@/shared/utils/enums';

const isToggled = ref(false);
const filters = ref<DataTableFilterMeta>();
const initFilters = () => {
  filters.value = {
    ticker: {
      operator: FilterOperator.OR,
      constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }],
    },
    tradeDate: {
      operator: FilterOperator.OR,
      constraints: [{ value: null, matchMode: FilterMatchMode.DATE_IS }],
    },
    avgPrice: {
      operator: FilterOperator.OR,
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
    },
    count: {
      operator: FilterOperator.OR,
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
    },
    sum: {
      operator: FilterOperator.OR,
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
    },
    currency: { value: null, matchMode: FilterMatchMode.IN },
    buySell: { value: null, matchMode: FilterMatchMode.IN },
    broker: { value: null, matchMode: FilterMatchMode.IN },
  };
};
const clearFilter = () => {
  initFilters();
};

initFilters();
const { data: seedMoney, isSuccess } = useSeedMoney();
const selected = ref();
const { data, isLoading } = useTrades({});
const trades = computed(() =>
  !isLoading.value ? data.value!.map((x) => ({ ...x, tradeDate: new Date(x.tradeDate!) })) : [],
);
</script>

<style scoped>
.my-field {
  padding-bottom: 40px;
  color: hsla(160, 100%, 37%, 1);
}
.all-hide {
  .my-hide,
  :deep(.p-datatable-column-title) {
    visibility: hidden;
    justify-content: center;
    &::before {
      content: '-';
      visibility: visible;
      color: red;
      font-size: xx-large;
      align-self: center;
    }
  }
  :deep(tr:nth-child(n + 1)) {
    color: rgba(0, 0, 0, 0);
  }
}
</style>
