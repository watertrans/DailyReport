import { createI18n } from 'vue-i18n';

const messages = {
  en: {
    menu: {
      dashborad: 'Dashborad',
      unreported: 'Not yet',
      unread: 'All unreads',
      thread: 'Threads',
      reaction: 'Mensions & reactions',
      bookmark: 'Saved items',
      search: 'Search',
      master: 'Master maintenance',
      masterGroup: 'Groups',
      masterProject: 'Projects',
      masterPerson: 'Persons',
      masterWorkType: 'Work types',
      analysis: 'Analysis',
      analysisGroup: 'Groups',
      analysisProject: 'Projects',
      analysisPerson: 'Persons',
      analysisWorkType: 'Work types',
      analysisLikeIt: 'Like it',
      analysisReaction: 'Reactions',
    }
  },
  ja: {
    menu: {
      dashborad: 'ダッシュボード',
      unreported: '未提出',
      unread: '全未読',
      thread: 'スレッド',
      reaction: 'メンション＆リアクション',
      bookmark: 'ブックマーク',
      search: '検索',
      master: 'マスタメンテ',
      masterGroup: '部署',
      masterProject: 'プロジェクト',
      masterPerson: '従業員',
      masterWorkType: '業務',
      analysis: '分析',
      analysisGroup: '部署',
      analysisProject: 'プロジェクト',
      analysisPerson: '従業員',
      analysisWorkType: '業務',
      analysisLikeIt: 'いいね',
      analysisReaction: 'リアクション',
    },
    general: {
      createButtonLabel: '新規登録',
      updateSelectedButtonLabel: '一括更新',
      selectPlaceholder: '選択してください',
      validationError: '入力エラーが発生しました。エラーメッセージを確認してください。',
      validationRequired: '\'{target}\'を入力してください。',
    },
    helpText: {
      dataCode: '英数字とハイフン20文字以内',
      dataTree: '数字2,4,6,8文字',
      text256: '256文字以内',
      tags: 'Enterキーで確定、最大10個',
    },
    dialog: {
      unauthorized: 'タイムアウト',
      unauthorizedMessage: 'アクセストークンがタイムアウトしました。認証画面に戻りますがよろしいですか？',
    },
    toast: {
      errorSummary: 'エラー',
      createSummary: '登録完了',
      createDetail: '登録処理が完了しました。',
      updateSummary: '更新完了',
      updateDetail: '更新処理が完了しました。',
      deleteSummary: '削除完了',
      deleteDetail: '削除処理が完了しました。',
    },
    dashboard: {
      unreadTitle: '未読日報',
      unreadDetail: '所属部署の日報の未読件数',
      unreportedTitle: '未提出日報',
      unreportedDetail: '所属部署の日報の未提出件数',
      workTypePieTitle: '今月の業務',
      workTypeLineTitle: '過去の業務',
      rankingTitle: 'ランキング',
      rankingTabReceivedTitle: 'いいね獲得数',
      rankingTabSentTitle: 'いいね送信数',
    },
    masterGroup: {
      createGroupTitle: '部署登録',
      updateGroupTitle: '部署更新',
    },
    schema: {
      group: {
        groupId: '部署ID',
        groupCode: '部署コード',
        groupTree: '部署階層',
        name: '部署名',
        description: '説明',
        status: 'ステータス',
        sortNo: '並び順',
        tags: 'タグ',
        persons: '所属従業員',
        createTime: '作成日時',
        updateTime: '更新日時',
      }
    }
  },
};

const i18n = createI18n({
  locale: 'ja',
  fallbackLocale: 'ja',
  messages: messages,
});

export default i18n;