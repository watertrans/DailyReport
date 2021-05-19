export default class PersonService {

  constructor(axios, token) {
    this.axios = axios;
    this.token = token;
  }

  queryPersons(queryParams) {
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
    if (queryParams.status) {
      params['status'] = queryParams.status;
    }
    if (queryParams.groupCode) {
      params['groupCode'] = queryParams.groupCode;
    }
    if (queryParams.projectCode) {
      params['projectCode'] = queryParams.projectCode;
    }
    return this.axios.get(process.env.VUE_APP_API_BASE_URL + '/persons', {
      params: params,
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  createPerson(person) {
    return this.axios.post(process.env.VUE_APP_API_BASE_URL + '/persons', person, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  updatePerson(person) {
    return this.axios.patch(process.env.VUE_APP_API_BASE_URL + '/persons/' + person.personId, person, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  deletePerson(personId) {
    return this.axios.delete(process.env.VUE_APP_API_BASE_URL + '/persons/' + personId, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }
}
