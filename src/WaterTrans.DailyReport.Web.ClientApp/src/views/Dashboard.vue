<template>
  <div class="p-grid p-fluid dashboard">
    <div class="p-col-12 p-lg-6">
      <div class="card summary">
        <span class="title">未読日報</span>
        <span class="detail">所属部署の日報の未読件数</span>
        <span class="count unread">12</span>
      </div>
    </div>
    <div class="p-col-12 p-lg-6">
      <div class="card summary">
        <span class="title">未提出日報</span>
        <span class="detail">所属部署の日報の未提出件数</span>
        <span class="count unreported">534</span>
      </div>
    </div>
    <div class="p-col-12 p-lg-6">
      <div class="card chart">
        <span class="title">今月の業務分類</span>
        <Chart type="pie" :data="pieData" />
      </div>
    </div>
    <div class="p-col-12 p-lg-6">
      <div class="card chart">
        <span class="title">過去の業務分類</span>
        <Chart type="line" :data="lineData" />
      </div>
    </div>
    <div class="p-col-12 p-lg-6">
      <div class="card ranking">
        <span class="title">ランキング</span>
        <TabMenu :model="nestedRouteItems" />
        <router-view/>
      </div>
    </div>
  </div>
</template>

<script>
import ReportService from '../service/ReportService';
export default {
  name: 'Dashboard',
  data() {
    return {
      products: null,
      productDialog: false,
      deleteProductDialog: false,
      deleteProductsDialog: false,
      product: {},
      selectedProducts: null,
      filters: {},
      submitted: false,
      statuses: [
        {label: 'INSTOCK', value: 'instock'},
        {label: 'LOWSTOCK', value: 'lowstock'},
        {label: 'OUTOFSTOCK', value: 'outofstock'}
      ],
      nestedRouteItems: [
        {
          label: 'いいね獲得数',
          to: '/'
        },
        {
          label: 'いいね送信数',
          to: '/dashboard/tabs/ranking/sentlikeit'
        }
      ],
      pieData: null,
      lineData: null
    };
  },
  reportService: null,
  created() {
    this.reportService = new ReportService();
  },
  mounted() {
    this.reportService.getDashboardWorkType().then(data => this.pieData = data);
    this.reportService.getDashboardWorkTypeHistory().then(data => this.lineData = data);
  },
};
</script>
