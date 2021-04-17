import axios from 'axios';
export default class RankingService {

  getRankingReceivedLikeIt() {
    return axios.get('/assets/data/getRankingReceivedLikeIt.json').then(res => res.data.data);
  }

  getRankingSentLikeIt() {
    return axios.get('/assets/data/getRankingSentLikeIt.json').then(res => res.data.data);
  }

}
