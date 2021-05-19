export default class WorkTypeService {

  constructor(axios, token) {
    this.axios = axios;
    this.token = token;
  }

  queryWorkTypes(queryParams) {
    var params = {};
    if (queryParams.page) {
      params['page'] = queryParams.page;
    }
    if (queryParams.pageSize) {
      params['pageSize'] = queryParams.pageSize;
    }
    if (queryParams.query) {
      params['query'] = queryParams.query;
    }
    if (queryParams.sort) {
      params['sort'] = queryParams.sort;
    }
    return this.axios.get(process.env.VUE_APP_API_BASE_URL + '/workTypes', {
      params: params,
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  createWorkType(workType) {
    return this.axios.post(process.env.VUE_APP_API_BASE_URL + '/workTypes', workType, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  updateWorkType(workType) {
    return this.axios.patch(process.env.VUE_APP_API_BASE_URL + '/workTypes/' + workType.workTypeId, workType, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  deleteWorkType(workTypeId) {
    return this.axios.delete(process.env.VUE_APP_API_BASE_URL + '/workTypes/' + workTypeId, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }
}
