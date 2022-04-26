function getTitle(vm: any) {
  const { title } = vm.$options;
  const { _route } = vm.$root;

  if (title) {
    return typeof title === 'function'
      ? title.call(vm)
      : title;
  }
  if (_route && _route.meta && _route.meta.title) {
    return _route.meta.title;
  }
}
export default {
  created() {
    const title = getTitle(this);
    if (title) {
      document.title = title;
    }
  },
};
