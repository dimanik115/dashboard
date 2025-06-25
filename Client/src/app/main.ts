import '../assets/main.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';
import PrimeVue from 'primevue/config';
import App from './App.vue';
import router from './router';
import Aura from '@primeuix/themes/aura';
import { all } from 'primelocale';
import Ripple from 'primevue/ripple';

const app = createApp(App);

app.directive('ripple', Ripple);
app.use(createPinia());
app.use(router);
app.use(PrimeVue, {
  theme: {
    preset: Aura,
  },
  ripple: true,
  locale:
    // all.ru
    all.en,
});
app.mount('#app');
