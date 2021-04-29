import axios from 'axios';
export default class ReportService {

  getDashboardWorkType() {
    return axios.get(process.env.VUE_APP_BASE_URL + '/assets/data/getDashboardWorkType.json').then(res => res.data.data);
  }

  getDashboardWorkTypeHistory() {
    return axios.get(process.env.VUE_APP_BASE_URL + '/assets/data/getDashboardWorkTypeHistory.json').then(res => res.data.data);
  }

}
