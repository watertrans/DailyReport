import axios from 'axios';
export default class ReportService {

  getDashboardWorkType() {
    return axios.get('/assets/data/getDashboardWorkType.json').then(res => res.data.data);
  }

  getDashboardWorkTypeHistory() {
    return axios.get('/assets/data/getDashboardWorkTypeHistory.json').then(res => res.data.data);
  }

}
