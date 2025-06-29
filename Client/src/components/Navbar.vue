<template>
  <div class="menu">
    <Menubar :model="items">
      <!--      <template #itemicon="{ item }">-->
      <!--        <ThLargeIcon v-if="item.label == 'Trades'"></ThLargeIcon>-->
      <!--        <ThLargeIcon v-else-if="item.label == 'Favourites'"></ThLargeIcon>-->
      <!--        <span v-else :class="item.icon"/>-->
      <!--      </template>-->
    </Menubar>
  </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue';
import { PrimeIcons } from '@primevue/core/api';
import { useRouter } from 'vue-router';
import type { MenuItem } from 'primevue/menuitem';

const router = useRouter();

const items = ref<MenuItem[]>([
  {
    label: 'Home',
    icon: PrimeIcons.HOME,
    command: () => router.push('/'),
  },
  {
    label: 'Statistics',
    icon: PrimeIcons.CHART_LINE,
    command: () => router.push('/statistics'),
  },
  {
    label: 'Trades',
    icon: PrimeIcons.TABLE,
    command: () => router.push('/trades'),
  },
  {
    label: 'Bookmarks',
    icon: PrimeIcons.BOOKMARK,
    command: () => router.push('/bookmarks'),
  },
]);
</script>

<style scoped>
.menu {
  --p-menubar-background: #e5ffde;
  padding-bottom: 40px;
}

.p-menubar {
  justify-content: center;
}
</style>
