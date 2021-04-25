import axios from 'axios';
export default class AuthService {

  getAccessToken(code) {
    var params = new URLSearchParams();
    params.append('grant_type', 'authorization_code');
    params.append('code', code);
    params.append('client_id', 'clientapp');
    return axios.post('https://localhost:44350/api/v1/token', params); // TODO 設定に移行する
  }

}
