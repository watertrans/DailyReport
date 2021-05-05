import 'primevue/resources/themes/saga-blue/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeflex/primeflex.css';
import 'primeicons/primeicons.css';
import './assets/layout/layout.scss';

import { createApp } from 'vue';
import router from './router';
import store from './store';
import i18n from './i18n';
import axios from './plugins/axios';
import utils from './plugins/utils';

import App from './App.vue';
import PrimeVue from 'primevue/config';
import Button from 'primevue/button';
import Chart from 'primevue/chart';
import Chip from 'primevue/chip';
import Chips from 'primevue/chips';
import Column from 'primevue/column';
import ConfirmDialog from 'primevue/confirmdialog';
import ConfirmPopup from 'primevue/confirmpopup';
import ConfirmationService from 'primevue/confirmationservice';
import DataTable from 'primevue/datatable';
import Dialog from 'primevue/dialog';
import Dropdown from 'primevue/dropdown';
import FileUpload from 'primevue/fileupload';
import InputText from 'primevue/inputtext';
import InputNumber from 'primevue/inputnumber';
import Message from 'primevue/message';
import OrganizationChart from 'primevue/organizationchart';
import RadioButton from 'primevue/radiobutton';
import Ripple from 'primevue/ripple';
import TabMenu from 'primevue/tabmenu';
import Textarea from 'primevue/textarea';
import Toast from 'primevue/toast';
import ToastService from 'primevue/toastservice';
import ToggleButton from 'primevue/togglebutton';
import Tooltip from 'primevue/tooltip';
import Toolbar from 'primevue/toolbar';

const app = createApp(App);

app.use(PrimeVue, { ripple: true });
app.use(ConfirmationService);
app.use(ToastService);
app.use(i18n);
app.use(store);
app.use(router);
app.use(axios);
app.use(utils);

app.directive('tooltip', Tooltip);
app.directive('ripple', Ripple);

app.component('Button', Button);
app.component('Chart', Chart);
app.component('Chip', Chip);
app.component('Chips', Chips);
app.component('Column', Column);
app.component('ConfirmDialog', ConfirmDialog);
app.component('ConfirmPopup', ConfirmPopup);
app.component('DataTable', DataTable);
app.component('Dialog', Dialog);
app.component('Dropdown', Dropdown);
app.component('FileUpload', FileUpload);
app.component('InputText', InputText);
app.component('InputNumber', InputNumber);
app.component('Message', Message);
app.component('OrganizationChart', OrganizationChart);
app.component('RadioButton', RadioButton);
app.component('TabMenu', TabMenu);
app.component('Textarea', Textarea);
app.component('Toast', Toast);
app.component('ToggleButton', ToggleButton);
app.component('Toolbar', Toolbar);

app.mount('#app');
