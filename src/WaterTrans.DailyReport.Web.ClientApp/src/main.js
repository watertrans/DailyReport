import 'primevue/resources/themes/saga-blue/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeflex/primeflex.css';
import 'primeicons/primeicons.css';
import './assets/layout/layout.scss';

import { createApp } from 'vue'
import router from './router'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import Ripple from 'primevue/ripple';
import Toast from 'primevue/toast';
import ToastService from 'primevue/toastservice';

const app = createApp(App);

app.use(PrimeVue, { ripple: true });
app.use(ToastService);
app.use(router);

app.directive('ripple', Ripple);

app.component('Toast', Toast);

app.mount('#app');
