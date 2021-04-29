import axios from 'axios';
export default class RankingService {

  getRankingReceivedLikeIt() {
    return axios.get(process.env.VUE_APP_BASE_URL + '/assets/data/getRankingReceivedLikeIt.json').then(res => res.data.data);
  }

  getRankingSentLikeIt() {
    return axios.get(process.env.VUE_APP_BASE_URL + '/assets/data/getRankingSentLikeIt.json').then(res => res.data.data);
  }

}
