export default class GroupService {

  constructor(axios, token) {
    this.axios = axios;
    this.token = token;
  }

  queryGroups(query, sort, page, pageSize) {
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
}
