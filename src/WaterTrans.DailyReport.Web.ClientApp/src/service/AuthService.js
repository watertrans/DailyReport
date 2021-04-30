export default class AuthService {

  constructor(axios, token) {
    this.axios = axios;
    this.token = token;
  }

  getAccessToken(code) {
    var params = new URLSearchParams();
    params.append('grant_type', 'authorization_code');
    params.append('code', code);
    params.append('client_id', process.env.VUE_APP_CLIENT_ID);
    return this.axios.post(process.env.VUE_APP_API_BASE_URL + '/token', params);
  }

}
