import axios from 'axios';

export default {
  
  install: (app, options) => {

    axios.interceptors.request.use(
      (request) => {
        return request;
      }
    );
    
    axios.interceptors.response.use(
      (response) => {
         // 2xx/3xx responses
         return response;
      },
      (error) => {
         // 4xx/5xx responses
         return Promise.reject(error);
      }
    );

    app.config.globalProperties.$axios = axios;
    app.provide('axios', options);

  }
};