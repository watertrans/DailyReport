import 'primevue/resources/themes/saga-blue/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeflex/primeflex.css';
import 'primeicons/primeicons.css';
import './assets/layout/layout.scss';

import { createApp } from 'vue';
import router from './router';
import App from './App.vue';
import PrimeVue from 'primevue/config';
import Chart from 'primevue/chart';
import Column from 'primevue/column';
import ConfirmDialog from 'primevue/confirmdialog';
import ConfirmPopup from 'primevue/confirmpopup';
import ConfirmationService from 'primevue/confirmationservice';
import DataTable from 'primevue/datatable';
import InputText from 'primevue/inputtext';
import Ripple from 'primevue/ripple';
import TabMenu from 'primevue/tabmenu';
import Toast from 'primevue/toast';
import ToastService from 'primevue/toastservice';
import Tooltip from 'primevue/tooltip';

const app = createApp(App);

app.use(PrimeVue, { ripple: true });
app.use(ConfirmationService);
app.use(ToastService);
app.use(router);

app.directive('tooltip', Tooltip);
app.directive('ripple', Ripple);

app.component('Chart', Chart);
app.component('Column', Column);
app.component('ConfirmDialog', ConfirmDialog);
app.component('ConfirmPopup', ConfirmPopup);
app.component('DataTable', DataTable);
app.component('InputText', InputText);
app.component('TabMenu', TabMenu);
app.component('Toast', Toast);

app.mount('#app');
