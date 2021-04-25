import { createStore } from 'vuex';

export default createStore({
  state: {
    accessToken: null,
    authorizationCode: null
  },
  mutations: {
    initialiseStore(state) {
      state.accessToken = localStorage.getItem('accessToken');
      state.authorizationCode = localStorage.getItem('authorizationCode');
    },
    setAccessToken(state, accessToken) {
      localStorage.setItem('accessToken', accessToken);
      state.accessToken = accessToken;
    },
    setAuthorizationCode(state, authorizationCode) {
      localStorage.setItem('authorizationCode', authorizationCode);
      state.authorizationCode = authorizationCode;
    }
  },
  actions: {
  },
  modules: {
  }
});
