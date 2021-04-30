<template>
  <DataTable :value="ranking" :loading="loading" class="p-mt-3">
    <template #header>
      集計期間：YYYY年MM月DD日 ~ YYYY年MM月DD日
    </template>
    <Column field="rank" header="順位" :style="{width:'70px'}"></Column>
    <Column field="name" header="名前"></Column>
    <Column field="activity" header="送信数" :style="{width:'100px', textAlign:'end'}"></Column>
  </DataTable>
</template>

<script>
import RankingService from '../../service/RankingService';
export default {
  data() {
    return {
      ranking: null,
      loading: true
    };
  },
  rankingService: null,
  created() {
    this.rankingService = new RankingService(this.$axios, this.$store.state.accessToken);
  },
  mounted() {
    this.rankingService.getRankingSentLikeIt().then(data => this.ranking = data);
    this.loading = false;
  },
};
</script>