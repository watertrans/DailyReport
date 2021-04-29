import axios from 'axios';
export default class AuthService {

  getAccessToken(code) {
    var params = new URLSearchParams();
    params.append('grant_type', 'authorization_code');
    params.append('code', code);
    params.append('client_id', process.env.VUE_APP_CLIENT_ID);
    return axios.post(process.env.VUE_APP_API_BASE_URL + '/token', params);
  }

}
