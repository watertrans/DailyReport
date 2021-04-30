<template>
  <div class="p-grid p-fluid dashboard">
    <div class="p-col-12 p-lg-6">
      <div class="card summary">
        <span class="title">{{ $t('dashboard.unreadTitle') }}</span>
        <span class="detail">{{ $t('dashboard.unreadDetail') }}</span>
        <span class="count unread">12</span>
      </div>
    </div>
    <div class="p-col-12 p-lg-6">
      <div class="card summary">
        <span class="title">{{ $t('dashboard.unreportedTitle') }}</span>
        <span class="detail">{{ $t('dashboard.unreportedDetail') }}</span>
        <span class="count unreported">534</span>
      </div>
    </div>
    <div class="p-col-12 p-lg-6">
      <div class="card chart">
        <span class="title">{{ $t('dashboard.workTypePieTitle') }}</span>
        <Chart type="pie" :data="pieData" />
      </div>
    </div>
    <div class="p-col-12 p-lg-6">
      <div class="card chart">
        <span class="title">{{ $t('dashboard.workTypeLineTitle') }}</span>
        <Chart type="line" :data="lineData" />
      </div>
    </div>
    <div class="p-col-12 p-lg-6">
      <div class="card ranking">
        <span class="title">{{ $t('dashboard.rankingTitle') }}</span>
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
      nestedRouteItems: [
        {
          label: this.$i18n.t('dashboard.rankingTabReceivedTitle'),
          to: '/'
        },
        {
          label: this.$i18n.t('dashboard.rankingTabSentTitle'),
          to: '/dashboard/tabs/ranking/sentlikeit'
        }
      ],
      pieData: null,
      lineData: null
    };
  },
  reportService: null,
  created() {
    this.reportService = new ReportService(this.$axios, this.$store.state.accessToken);
  },
  mounted() {
    this.reportService.getDashboardWorkType().then(data => this.pieData = data);
    this.reportService.getDashboardWorkTypeHistory().then(data => this.lineData = data);
  },
};
</script>
