export default class ReportService {

  constructor(axios, token) {
    this.axios = axios;
    this.token = token;
  }

  getDashboardWorkType() {
    return this.axios.get(process.env.VUE_APP_BASE_URL + '/assets/data/getDashboardWorkType.json').then(res => res.data.data);
  }

  getDashboardWorkTypeHistory() {
    return this.axios.get(process.env.VUE_APP_BASE_URL + '/assets/data/getDashboardWorkTypeHistory.json').then(res => res.data.data);
  }

}
