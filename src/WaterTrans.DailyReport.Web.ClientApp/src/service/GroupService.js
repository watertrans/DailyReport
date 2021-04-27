import axios from 'axios';
export default class GroupService {

  constructor(token) {
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
    return axios.get('https://localhost:44350/api/v1/groups', {
      params: params,
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    }); // TODO 設定に移行する
  }
}
