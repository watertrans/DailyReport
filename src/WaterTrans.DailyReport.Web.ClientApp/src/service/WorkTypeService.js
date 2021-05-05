export default class WorkTypeService {

  constructor(axios, token) {
    this.axios = axios;
    this.token = token;
  }

  queryWorkTypes(query, sort, page, pageSize) {
    var params = {};
    if (page) {
      params['page'] = page;
    }
    if (pageSize) {
      params['pageSize'] = pageSize;
    }
    if (query) {
      params['query'] = query;
    }
    if (sort) {
      params['sort'] = sort;
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
