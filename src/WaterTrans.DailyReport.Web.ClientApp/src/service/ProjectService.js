export default class ProjectService {

  constructor(axios, token) {
    this.axios = axios;
    this.token = token;
  }

  queryProjects(query, sort, page, pageSize) {
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
    return this.axios.get(process.env.VUE_APP_API_BASE_URL + '/projects', {
      params: params,
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  createProject(project) {
    return this.axios.post(process.env.VUE_APP_API_BASE_URL + '/projects', project, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  updateProject(project) {
    return this.axios.patch(process.env.VUE_APP_API_BASE_URL + '/projects/' + project.projectId, project, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }

  deleteProject(projectId) {
    return this.axios.delete(process.env.VUE_APP_API_BASE_URL + '/projects/' + projectId, {
      headers: {
        Authorization: `Bearer ${this.token}`,
      }
    });
  }
}
