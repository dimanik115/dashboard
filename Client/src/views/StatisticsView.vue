<template>
  <div>
    <div class="top-data">
      <div class="first-col">
        <div class="my-field">
          <h3>
            Всего куплено:
            <span v-if="isSuccessTotalMoney">
              {{ isHide ? 0 : formatCurrency(totalMoney!, { maxFractionDigits: 0 }) }}
            </span>
          </h3>
          <h3>
            Доход примерно 1% в мес:
            {{ isHide ? 0 : formatCurrency(totalMoney! / 100, { maxFractionDigits: 0 }) }}
          </h3>
          <h3 v-if="isLive">
            Всего на сейчас:
            <span v-if="isSuccessTotalMoneyNow">
              {{ isHide ? 0 : formatCurrency(totalMoneyNow!, { maxFractionDigits: 0 }) }}
            </span>
          </h3>
        </div>
        <div class="seed-and-cur-tables">
          <div class="seed-table">
            <DataTable :value="seedMoney">
              <Column field="broker" header="broker">
                <template #body="{ data }">
                  {{ isHide ? '*' : data.broker }}
                </template>
              </Column>
              <Column field="seedMoney" header="seedMoney">
                <template #body="{ data }">
                  {{ isHide ? '*' : formatCurrency(data.seedMoney, { maxFractionDigits: 0 }) }}
                </template>
              </Column>
              <template #footer v-if="isSuccessSeedMoney">
                <i>
                  Всего вложено:
                  {{
                    isHide
                      ? 0
                      : formatCurrency(
                          seedMoney!.reduce((prev, cur) => prev + cur.seedMoney!, 0),
                          { maxFractionDigits: 0 },
                        )
                  }}
                </i>
              </template>
            </DataTable>
          </div>
          <div class="currency-table">
            <DataTable :value="avgPrices">
              <Column field="currency" header="currency">
                <template #body="{ data }">
                  {{ isHide ? '*' : data.currency }}
                </template>
              </Column>
              <Column field="avgPrice" header="avgPrice">
                <template #body="{ data }">
                  {{ isHide ? '*' : formatCurrency(data.avgPrice) }}
                </template>
              </Column>
              <Column v-if="isLive && !isHide" field="avgPriceNow" header="avgPriceNow">
                <template #body="{ data }">
                  {{ isHide ? '*' : formatCurrency(data.avgPriceNow) }}
                </template>
              </Column>
            </DataTable>
          </div>
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
      :rows="pageSize"
      v-model:first="page"
      v-model:sort-field="sort"
      :rowsPerPageOptions="[5, 15, 30]"
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

      <Column field="ticker" header="Ticker" sortable body-style="padding:0">
        <template #body="{ data }">
          <span
            v-if="!isHide"
            class="ticker-color"
            :style="{ 'background-color': hexToRGBA(data.color ?? '#FFFFFF', 0.6) }">
            <b>{{ data.ticker }}</b>
          </span>
          <span v-else>*</span>
        </template>
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
        <template #body="{ data }">
          {{ isHide ? '*' : data.sumCount }}
        </template>
        <template #filter="{ filterModel }">
          <InputNumber v-model="filterModel.value" mode="decimal" />
        </template>
      </Column>
      <Column field="total" header="total" dataType="numeric" sortable sortField="sumRubNow">
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
        <template #body="{ data }">
          {{ isHide ? '*' : data.currency }}
        </template>
        <template #filter="{ filterModel }">
          <MultiSelect v-model="filterModel.value" :options="useCurrencyOptions()" placeholder="Any" />
        </template>
      </Column>
      <Column field="country" header="Country" :showFilterMatchModes="false" sortable>
        <template #body="{ data }">
          {{ isHide ? '*' : data.country }}
        </template>
        <template #filter="{ filterModel }">
          <MultiSelect v-model="filterModel.value" :options="countryStat?.map((x) => x.country)" placeholder="Any" />
        </template>
      </Column>
      <Column field="sector" header="Sector" :showFilterMatchModes="false" sortable>
        <template #body="{ data }">
          {{ isHide ? '*' : data.sector }}
        </template>
        <template #filter="{ filterModel }">
          <MultiSelect v-model="filterModel.value" :options="sectorStat?.map((x) => x.sector)" placeholder="Any" />
        </template>
      </Column>
      <Column field="type" header="Type" :showFilterMatchModes="false" sortable>
        <template #body="{ data }">
          {{ isHide ? '*' : data.type }}
        </template>
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
import type { Statistics } from '@/shared/generated';
import { useCurrencyOptions } from '@/shared/utils/enums';
import { formatCurrency } from '@/shared/utils/num';
import { useLocalStorage } from '@/shared/utils/useLocalStorage';
import { hexToRGBA } from '@/shared/utils/useRgba';
import { FilterMatchMode, FilterOperator, PrimeIcons } from '@primevue/core/api';
import type { DataTableFilterMeta, DataTableRowSelectEvent } from 'primevue/datatable';
import { computed, ref } from 'vue';

const search = ref('source:ticker');
const isLive = useLocalStorage('isLive', false);
const pageSize = ref(15);
const page = ref(0);
const sort = ref('');
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
  pageSize.value = 15;
  page.value = 0;
  sort.value = '';
};
const clearFilter = () => {
  initFilters();
};

initFilters();
const { data: seedMoney, isSuccess: isSuccessSeedMoney } = useSeedMoney();
const { data: totalMoney, isSuccess: isSuccessTotalMoney } = useTotalMoney(false);
const { data: totalMoneyNow, isSuccess: isSuccessTotalMoneyNow } = useTotalMoney(isLive);

const selected = ref();
const { data: statistics } = useStatistics(isLive);
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
  search.value = event.data.source + ':' + event.data.ticker?.split(' ')[0];
}
</script>

<style scoped>
.my-field {
  color: hsla(160, 100%, 37%, 1);
}

.top-data {
  display: flex;
  /* align-items: center; */
  padding-bottom: 40px;
}

.seed-and-cur-tables {
  display: flex;
  border: ridge hsla(160, 100%, 37%, 1);
}

.seed-table {
  padding-right: 80px;
}
.currency-table {
  padding-right: 40px;
}

.ministat-table {
  margin: auto;
}

.ticker-color {
  padding: 10px 0px 10px 10px;
  display: flex;
  border-radius: 10px;
}
</style>
