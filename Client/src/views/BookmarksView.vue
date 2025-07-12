<template>
  <div>
    <DataTable
      v-model:filters="filters"
      v-model:expandedRowGroups="expandedRowGroups"
      expandableRowGroups
      rowGroupMode="subheader"
      groupRowsBy="group"
      @rowgroup-expand="isShow = true"
      @rowgroup-collapse="isShow = false"
      :value="data"
      removableSort>
      <template #header>
        <div class="header">
          <IconField>
            <InputIcon>
              <i class="pi pi-search" />
            </InputIcon>
            <InputText v-model="filters['global'].value" placeholder="Keyword Search" />
          </IconField>
          <Button text icon="pi pi-plus" label="Expand All" @click="expandAll" />
          <Button text icon="pi pi-minus" label="Collapse All" @click="collapseAll" />
        </div>
      </template>
      <template #groupheader="slotProps">
        <strong>{{ slotProps.data.group.toUpperCase() }}</strong>
      </template>
      <template #empty>No data found.</template>

      <Column
        v-for="(col, index) in columns"
        :key="index"
        :field="col.field"
        :header="col.header"
        :sortable="col.sortable"></Column>

      <Column v-if="isShow" header="url">
        <template #body="{ data }">
          <a
            target="_blank"
            rel="noopener noreferrer"
            :href="`https://www.tradingview.com/chart/?symbol=${data.source}:${data.ticker}`">
            ссылка на tradingview
          </a>
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { FilterMatchMode } from '@primevue/core/api';
import { useBookmarks } from '@/shared/api/BookmarksApi';

const isShow = ref(false);
const columns = [
  { field: 'source', header: 'Source', sortable: true },
  { field: 'ticker', header: 'Ticker', sortable: true },
  { field: 'type', header: 'Type', sortable: true },
  { field: 'country', header: 'country', sortable: true },
  { field: 'sector', header: 'sector', sortable: true },
  { field: 'isBought', header: 'isBought', sortable: true },
  { field: 'description', header: 'description', sortable: true },
];
const expandedRowGroups = ref();
const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS },
});
const { data } = useBookmarks();
function expandAll() {
  expandedRowGroups.value = data.value?.reduce(
    (arr, next) => {
      if (!arr.includes(next.group)) arr.push(next.group);
      return arr;
    },
    [] as Array<string | undefined>,
  );
  isShow.value = true;
  console.log(expandedRowGroups.value);
}
const collapseAll = () => {
  expandedRowGroups.value = undefined;
  isShow.value = false;
};
</script>

<style scoped>
.header {
  display: flex;
}
</style>
