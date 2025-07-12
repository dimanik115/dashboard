<template>
  <div>
    <!-- TradingView Widget BEGIN -->
    <div class="tradingview-widget-container" style="height: 700px; padding-top: 100px">
      <div class="tradingview-widget-container__widget" style="height: 400px"></div>
    </div>
    <!-- TradingView Widget END -->
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
const props = defineProps<{ search: string }>();

onMounted(() => {
  const div = document.querySelector('.tradingview-widget-container')!;
  const script = document.createElement('script');
  script.src = 'https://s3.tradingview.com/external-embedding/embed-widget-advanced-chart.js';
  script.type = 'text/javascript';
  script.async = true;
  script.innerHTML = `
        {
          "allow_symbol_change": true,
          "calendar": false,
          "details": false,
          "hide_side_toolbar": true,
          "hide_top_toolbar": false,
          "hide_legend": false,
          "hide_volume": false,
          "hotlist": false,
          "interval": "D",
          "locale": "en",
          "save_image": true,
          "style": "1",
          "symbol": "NASDAQ:AAPL",
          "theme": "light",
          "timezone": "Etc/UTC",
          "backgroundColor": "#ffffff",
          "gridColor": "rgba(46, 46, 46, 0.06)",
          "watchlist": [],
          "withdateranges": false,
          "compareSymbols": [],
          "studies": [],
          "autosize": true
        }`;
  div.appendChild(script);
});
</script>

<style scoped></style>
