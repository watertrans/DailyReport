export default class PersonService {

  constructor(axios, token) {
    this.axios = axios;
    this.token = token;
  }

  queryPersons(query, sort, page, pageSize) {
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
