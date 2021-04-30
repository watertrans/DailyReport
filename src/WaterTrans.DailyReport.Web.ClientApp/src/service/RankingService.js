export default class RankingService {

  constructor(axios, token) {
    this.axios = axios;
    this.token = token;
  }

  getRankingReceivedLikeIt() {
    return this.axios.get(process.env.VUE_APP_BASE_URL + '/assets/data/getRankingReceivedLikeIt.json').then(res => res.data.data);
  }

  getRankingSentLikeIt() {
    return this.axios.get(process.env.VUE_APP_BASE_URL + '/assets/data/getRankingSentLikeIt.json').then(res => res.data.data);
  }

}
