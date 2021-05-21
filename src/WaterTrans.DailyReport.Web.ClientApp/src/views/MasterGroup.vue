<template>
  <div class="p-grid masterGroup">
    <div class="p-col-12">
      <div class="card">
        <Toast/>
        <Toolbar class="p-mb-4">
          <template v-slot:left>
            <Button :label="$t('general.createButtonLabel')" icon="pi pi-plus" class="p-button-success p-mr-2" @click="openNew" />
            <Button :label="$t('general.updateSelectedButtonLabel')" icon="pi pi-tags" class="p-button-success p-mr-2" @click="updateSelected" :disabled="!selectedGroups || !selectedGroups.length" />
            <span class="p-input-icon-left">
              <i class="pi pi-search" />
              <InputText v-model="query" :placeholder="$t('general.search')" @keydown.enter="onSearchKeyDown" />
            </span>
          </template>

          <template v-slot:right>
            <Button :label="$t('masterGroup.viewOrganization')" icon="pi pi-sitemap" class="p-button-info p-mr-2" @click="viewOrganization"  />
            <Button icon="pi pi-upload" class="p-button-success p-mr-2" @click="showUploadDialog"  />
            <Button icon="pi pi-download" class="p-button-success" @click="exportCSV"  />
          </template>
        </Toolbar>

        <DataTable ref="dt" :value="groups" v-model:selection="selectedGroups" dataKey="groupId" :lazy="true" :paginator="true" :rows="rows" :first="first"
              :totalRecords="totalRecords" :loading="loading" :resizableColumns="true" :removableSort="true" :sortOrder="sortOrder" :sortField="sortField"
              paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[5,10,20,50,100]"
              currentPageReportTemplate="Showing {first} to {last} of {totalRecords} groups" responsiveLayout="scroll" @page="onPage" @sort="onPage">
          <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
          <Column field="groupCode" :header="$t('schema.group.groupCode')" headerStyle="width: 10rem" :sortable="true"></Column>
          <Column field="groupTree" :header="$t('schema.group.groupTree')" headerStyle="width: 6rem" :sortable="true"></Column>
          <Column field="name" :header="$t('schema.group.name')" :sortable="true"></Column>
          <Column field="tags" :header="$t('schema.group.tags')" headerStyle="width: 15rem">
            <template #body="slotProps">
              <Chip v-for="tag in slotProps.data.tags" :key="tag" :label="tag" />
            </template>
          </Column>
          <Column field="status" :header="$t('schema.group.status')" headerStyle="width: 8rem"></Column>
          <Column field="sortNo" :header="$t('schema.group.sortNo')" headerStyle="width: 8rem" :sortable="true"></Column>
          <Column field="persons.length" :header="$t('schema.group.persons')" headerStyle="width: 8rem"></Column>
          <Column headerStyle="width: 10rem">
            <template #body="slotProps">
              <Button icon="pi pi-pencil" class="p-button-rounded p-button-outlined p-button-success p-mr-2" @click="editGroup(slotProps.data)" />
              <Button icon="pi pi-trash" class="p-button-rounded p-button-outlined p-button-warning" @click="confirmDeleteGroup(slotProps.data)" :disabled="slotProps.data.persons.length > 0" />
            </template>
          </Column>
        </DataTable>

        <Dialog v-model:visible="fileUploadDialog" :style="{width: '450px'}" :header="$t('masterGroup.fileUploadDialogHeader')" :modal="true">
          <FileUpload mode="basic" :customUpload="true" :auto="true" accept="text/csv" :maxFileSize="1000000" :chooseLabel="$t('general.fileUploadDialogChooseButtonLabel')" @uploader="importCSV" class="p-d-inline-block" />
          <template #footer>
            <Button :label="$t('general.fileUploadDialogColumnsButtonLabel')" icon="pi pi-question-circle" class="p-button-text" />
            <Button :label="$t('general.fileUploadDialogDownloadSampleButtonLabel')" icon="pi pi-download" class="p-button-text" />
          </template>
        </Dialog>

        <Dialog v-model:visible="groupDialog" :style="{width: '450px'}" :header="groupDialogHeader" :modal="true" class="p-fluid">
          <Message v-if="error && error.message" severity="error" :closable="false">{{error.message}}</Message>
          <div class="p-formgrid p-grid">
            <div class="p-field p-col">
              <label for="groupCode">{{$t('schema.group.groupCode')}}</label>
              <InputText id="groupCode" v-model="group.groupCode" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'groupCode') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'groupCode')">{{error.details.find(e => e.target == 'groupCode').message}}</small>
              <small class="help-text">{{$t('helpText.dataCode')}}</small>
            </div>
            <div class="p-field p-col">
              <label for="groupTree">{{$t('schema.group.groupTree')}}</label>
              <InputText id="groupTree" v-model="group.groupTree" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'groupTree') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'groupTree')">{{error.details.find(e => e.target == 'groupTree').message}}</small>
              <small class="help-text">{{$t('helpText.dataTree')}}</small>
            </div>
          </div>
          <div class="p-field">
            <label for="name">{{$t('schema.group.name')}}</label>
            <InputText id="name" v-model="group.name" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'name') }" />
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'name')">{{error.details.find(e => e.target == 'name').message}}</small>
            <small class="help-text">{{$t('helpText.text256')}}</small>
          </div>
          <div class="p-field">
            <label for="description">{{$t('schema.group.description')}}</label>
            <Textarea id="description" v-model="group.description" rows="3" cols="20" />
          </div>
          <div class="p-formgrid p-grid">
            <div class="p-field p-col">
              <label for="status">{{$t('schema.group.status')}}</label>
              <Dropdown id="status" v-model="group.status" :options="statuses" optionLabel="label" optionValue="value" :placeholder="$t('general.selectPlaceholder')" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'status') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'status')">{{error.details.find(e => e.target == 'status').message}}</small>
            </div>
            <div class="p-field p-col">
              <label for="sortNo">{{$t('schema.group.sortNo')}}</label>
              <InputNumber id="sortNo" v-model="group.sortNo" :useGrouping="false" :min="0" :max="2147483647" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'sortNo') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'sortNo')">{{error.details.find(e => e.target == 'sortNo').message}}</small>
            </div>
          </div>
          <div class="p-field">
            <label for="tags">{{$t('schema.group.tags')}}</label>
            <Chips id="tags" v-model="group.tags" :addOnBlur="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'tags') }" />
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'tags')">{{error.details.find(e => e.target == 'tags').message}}</small>
            <small class="help-text">{{$t('helpText.tags')}}</small>
          </div>
          <template #footer>
            <Button :label="$t('dialog.cancelButtonLabel')" icon="pi pi-times" class="p-button-text" @click="groupDialog = false"/>
            <Button :label="$t('dialog.saveButtonLabel')" icon="pi pi-check" class="p-button-text" @click="saveGroup" />
          </template>
        </Dialog>

        <Dialog v-model:visible="deleteGroupDialog" :style="{width: '450px'}" :header="$t('general.deleteComfirmTitle')" :modal="true">
          <div class="confirmation-content">
            <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
            <span v-if="group">{{$t('general.deleteComfirmMessage', { target : group.name })}}</span>
          </div>
          <template #footer>
            <Button :label="$t('dialog.noButtonLabel')" icon="pi pi-times" class="p-button-text" @click="deleteGroupDialog = false"/>
            <Button :label="$t('dialog.yesButtonLabel')" icon="pi pi-check" class="p-button-text" @click="deleteGroup" />
          </template>
        </Dialog>

        <Dialog v-model:visible="updateSelectedDialog" :style="{width: '450px'}" :header="$t('masterGroup.updateSelectedGroupTitle')" :modal="true" class="p-fluid">
          <Message v-if="error && error.message" severity="error" :closable="false">{{error.message}}</Message>
          <div class="p-field">
            <label for="status2">{{$t('schema.group.status')}}</label>
            <div class="p-formgrid p-grid">
              <div class="p-col-fixed">
                <ToggleButton v-model="updateSelectedGroup.statusChecked" onIcon="pi pi-check" offIcon="pi pi-times" />
              </div>
              <div class="p-col">
                <Dropdown id="status2" v-model="updateSelectedGroup.status" :disabled="!updateSelectedGroup.statusChecked" :options="statuses" optionLabel="label" optionValue="value" :placeholder="$t('general.selectPlaceholder')" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'status') }" :style="{width: '100%'}" />
              </div>
            </div>
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'status')">{{error.details.find(e => e.target == 'status').message}}</small>
          </div>
          <div class="p-field">
            <label for="tags2">{{$t('schema.group.tags')}}</label>
            <div class="p-formgrid p-grid">
              <div class="p-col-fixed">
                <ToggleButton v-model="updateSelectedGroup.tagsChecked" onIcon="pi pi-check" offIcon="pi pi-times" />
              </div>
              <div class="p-col">
                <Chips id="tags2" v-model="updateSelectedGroup.tags" :disabled="!updateSelectedGroup.tagsChecked" :addOnBlur="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'tags') }" />
              </div>
            </div>
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'tags')">{{error.details.find(e => e.target == 'tags').message}}</small>
            <small class="help-text">{{$t('helpText.tags')}}</small>
          </div>
          <template #footer>
            <Button :label="$t('dialog.cancelButtonLabel')" icon="pi pi-times" class="p-button-text" @click="updateSelectedDialog = false"/>
            <Button :label="$t('dialog.updateButtonLabel')" icon="pi pi-check" class="p-button-text" @click="updateSelectedGroups" />
          </template>
        </Dialog>

        <Dialog :header="$t('masterGroup.viewOrganization')" v-model:visible="organizationDialog" :style="{width: '75vw'}" :maximizable="true" :modal="true">
          <OrganizationChart :value="organization" :collapsible="true" class="organization">
            <template #default="slotProps">
              <div class="node-header">{{slotProps.node.data.name}}</div>
              <div class="node-content">
                <small v-for="manager in slotProps.node.data.managers" :key="manager" class="node-manager">{{manager.name}}</small>
              </div>
            </template>
          </OrganizationChart>
        </Dialog>
      </div>
    </div>
  </div>

</template>

<script>
import ErrorHandling from '../mixins/ErrorHandling';
import GroupService from '../service/GroupService';

export default {
  data() {
    return {
      groups: null,
      groupDialog: false,
      groupDialogHeader: null,
      fileUploadDialog: false,
      deleteGroupDialog: false,
      updateSelectedDialog: false,
      organizationDialog: false,
      group: {},
      updateSelectedGroup: {},
      selectedGroups: null,
      query: null,
      sortOrder: 1,
      sortField: null,
      first: 0,
      rows: 20,
      totalRecords: 0,
      loading: true,
      error: null,
      statuses: [
        {label: 'NORMAL', value: 'NORMAL'},
        {label: 'SUSPENDED', value: 'SUSPENDED'}
      ],
      organization: { key: '0', data: { name: 'loading...'}},
    };
  },
  mixins: [ErrorHandling],
  groupService: null,
  created() {
    this.groupService = new GroupService(this.$axios, this.$store.state.accessToken);
    if (!this.$route.query) {
      return;
    }
    if (this.$route.query.query) {
      this.query = this.$route.query.query;
    }
    if (this.$route.query.sort) {
      const firstChar = this.$route.query.sort.substring(0,1);
      if (firstChar === '-') {
        this.sortField = this.$route.query.sort.replace('-','');
        this.sortOrder = -1;
      } else {
        this.sortField = this.$route.query.sort;
        this.sortOrder = 1;
      }
    }
    if (this.$route.query.pageSize && !isNaN(this.$route.query.pageSize)) {
      this.rows = parseInt(this.$route.query.pageSize);
    }
    if (this.$route.query.page && !isNaN(this.$route.query.page)) {
      this.first = (this.rows * parseInt(this.$route.query.page)) - 1;
    }
  },
  mounted() {
    this.reloadDataTable();
  },
  methods: {
    onSearchKeyDown(event) {
      if (!event || !event.isComposing) {
        this.reloadDataTable();
      }
    },
    onPage(event) {
      var page = 1;
      if (!isNaN(event.page)) {
        page = event.page + 1;
      }
      this.loadDataTable(this.convertToSortReuqest(event), page);
    },
    reloadDataTable() {
      this.loadDataTable(this.convertToSortReuqest({ 
        sortField: this.$refs.dt.d_sortField,
        sortOrder: this.$refs.dt.d_sortOrder
      }), Math.floor(this.$refs.dt.first / this.$refs.dt.d_rows) + 1);
    },
    loadDataTable(sort = null, page = 1) {
      const queryParams = {};
      const pageSize = this.$refs.dt.d_rows;
      if (this.query) {
        queryParams.query = this.query;
      }
      if (sort) {
        queryParams.sort = sort;
      }
      if (page != 1) {
        queryParams.page = page;
      }
      if (pageSize != 20) {
        queryParams.pageSize = pageSize;
      }
      this.$router.push({ query: queryParams });
      this.queryGroups(queryParams);
    },
    queryGroups(queryParams) {
      this.loading = true;
      this.groupService.queryGroups(queryParams)
        .then(response => {
          this.groups = response.data.items;
          this.rows = response.data.pageSize;
          this.totalRecords = response.data.total;
          this.loading = false;
        })
        .catch(error => this.handleErrorAndToastMessage(error));
    },
    openNew() {
      this.error = null;
      this.group = { description: '', tags: [] };
      this.groupDialogHeader = this.$i18n.t('masterGroup.createGroupTitle');
      this.groupDialog = true;
    },
    editGroup(group) {
      this.error = null;
      this.group = {...group};
      this.groupDialogHeader = this.$i18n.t('masterGroup.updateGroupTitle');
      this.groupDialog = true;
    },
    saveGroup() {
      this.error = { details: [] };

      if (!this.group.groupCode) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'groupCode', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.group.groupCode')}) });
      }
      if (!this.group.groupTree) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'groupTree', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.group.groupTree')}) });
      }
      if (!this.group.name) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'name', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.group.name')}) });
      }
      if (!this.group.status) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'status', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.group.status')}) });
      }
      if (!this.group.sortNo && this.group.sortNo !== 0) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'sortNo', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.group.sortNo')}) });
      }

      if (this.error.message) {
        return;
      }

      if (this.group.groupId) {
        this.groupService.updateGroup(this.group)
          .then(response => {
            this.reloadDataTable();
            this.$toast.add({severity:'success', summary: this.$i18n.t('toast.updateSummary'), detail: this.$i18n.t('toast.updateDetail'), life: 5000});
            this.groupDialog = false;
            this.group = response.data;
          })
          .catch(error => this.handleErrorAndDiplayMessage(error));
      } else {
        this.groupService.createGroup(this.group)
          .then(response => {
            this.reloadDataTable();
            this.$toast.add({severity:'success', summary: this.$i18n.t('toast.createSummary'), detail: this.$i18n.t('toast.createDetail'), life: 5000});
            this.groupDialog = false;
            this.group = response.data;
          })
          .catch(error => this.handleErrorAndDiplayMessage(error));
      }
    },
    confirmDeleteGroup(group) {
      this.group = group;
      this.deleteGroupDialog = true;
    },
    deleteGroup() {
      this.groupService.deleteGroup(this.group.groupId)
        .then(() => {
          this.reloadDataTable();
          this.$toast.add({severity:'success', summary: this.$i18n.t('toast.deleteSummary'), detail: this.$i18n.t('toast.deleteDetail'), life: 5000});
          this.deleteGroupDialog = false;
          this.group = {};
        })
        .catch(error => {
          this.handleErrorAndToastMessage(error);
          this.reloadDataTable();
          this.deleteGroupDialog = false;
          this.group = {};
        });
    },
    exportCSV() {
      console.log('Not implemented!'); // TODO
    },
    showUploadDialog() {
      this.fileUploadDialog = true;
    },
    importCSV() {
      console.log('Not implemented!'); // TODO
    },
    updateSelected() {
      this.error = null;
      this.updateSelectedGroup = { tags: []};
      this.updateSelectedDialog = true;
    },
    updateSelectedGroups() {
      this.error = { details: [] };

      if (!this.updateSelectedGroup.statusChecked && !this.updateSelectedGroup.tagsChecked) {
        this.error.message = this.$i18n.t('general.updateSelectedRequired');
      }

      if (this.updateSelectedGroup.statusChecked && !this.updateSelectedGroup.status) {
        this.error.message = this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.group.status')});
      }

      if (this.error.message) {
        return;
      }

      var promises = [];
      this.selectedGroups.forEach(group => {
        var updateGroup = { groupId: group.groupId };
        if (this.updateSelectedGroup.statusChecked) {
          updateGroup.status = this.updateSelectedGroup.status;
        }
        if (this.updateSelectedGroup.tagsChecked) {
          updateGroup.tags = this.updateSelectedGroup.tags;
        }
        promises.push(
          this.groupService.updateGroup(updateGroup)
        );
      });

      Promise.all(promises)
        .then(() => {
          this.$toast.add({severity:'success', summary: this.$i18n.t('toast.updateSummary'), detail: this.$i18n.t('toast.updateDetail'), life: 5000});
          this.reloadDataTable();
          this.updateSelectedDialog = false;
          this.updateSelectedGroup = {};
          this.selectedGroups = null;
        })
        .catch(error => {
          this.handleErrorAndToastMessage(error);
          this.reloadDataTable();
          this.updateSelectedDialog = false;
          this.updateSelectedGroup = {};
        });
    },
    viewOrganization() {
      this.organizationDialog = true;
      this.groupService.hierarchy()
        .then((response) => {
          response.data.key = response.data.groupId;
          response.data.data = { name: 'CEO' };
          response.data.children.forEach(element => this.convertOrganization(element));
          this.organization = response.data;
        })
        .catch(error => this.handleErrorAndToastMessage(error));
    },
    convertOrganization(node) {
      node.key = node.groupId;
      node.data = { name: node.name, managers: node.managers };
      node.children.forEach(element => this.convertOrganization(element));
    }
  }
};
</script>
