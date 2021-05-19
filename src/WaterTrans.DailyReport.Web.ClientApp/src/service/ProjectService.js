export default class ProjectService {

  constructor(axios, token) {
    this.axios = axios;
    this.token = token;
  }

  queryProjects(queryParams) {
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
