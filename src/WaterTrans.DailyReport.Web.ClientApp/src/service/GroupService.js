export default class GroupService {

  constructor(axios, token) {
    this.axios = axios;
    this.token = token;
  }

  queryGroups(queryParams) {
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
    return this.axios.get(process.env.VUE_APP_API_BASE_URL + '/groups', {
      params: params,
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  createGroup(group) {
    return this.axios.post(process.env.VUE_APP_API_BASE_URL + '/groups', group, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  updateGroup(group) {
    return this.axios.patch(process.env.VUE_APP_API_BASE_URL + '/groups/' + group.groupId, group, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  deleteGroup(groupId) {
    return this.axios.delete(process.env.VUE_APP_API_BASE_URL + '/groups/' + groupId, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  hierarchy() {
    return this.axios.get(process.env.VUE_APP_API_BASE_URL + '/groups/hierarchy', {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }
}
