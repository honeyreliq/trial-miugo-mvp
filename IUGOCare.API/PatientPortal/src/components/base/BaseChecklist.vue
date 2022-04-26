<template>
  <label class="check-item">
    <input type="checkbox" :checked="checked" v-bind="$attrs" />
    <span class="checkmark"></span>
    <span class="check-label text-body-1">
      <slot />
    </span>
  </label>
</template>

<script>
export default {
  name: 'BaseChecklist',

  props: {
    checked: Boolean,
  },
};
</script>

<style lang="scss" scoped>
.check-item {
  display: block;
  position: relative;
  padding-left: 32px;
  margin-bottom: 16px;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
  cursor: pointer;
}

.check-item[disabled="disabled"]{
  cursor: default;
}

/* Hide the browser's default checkbox */
.check-item input {
  position: absolute;
  opacity: 0;
  cursor: pointer;
  height: 0;
  width: 0;
}

/* Create a custom checkbox */
.checkmark {
  position: absolute;
  top: 0;
  left: 0;
  height: 24px;
  width: 24px;
  background-color: transparent;
  border-radius: 15px;
  border-width: 1px;
  border-style: solid;
}

/* When the checkbox is checked, add a blue background */
.check-item input:checked ~ .checkmark {
  background-color: var(--v-primary-base);
  border-color: var(--v-primary-base);
}

/* Create the checkmark/indicator */
.checkmark:after {
  content: "\00AC";
  position: absolute;
}

/* Color the checkmark when checked */
.check-item input:checked ~ .checkmark:after {
  color:white;
}

/* Style the checkmark/indicator */
.check-item .checkmark:after {
  left: 17px;
  top: 7px;
  width: 5px;
  height: 10px;
  font-size: 24px;
  -webkit-transform: rotate(135deg);
  -ms-transform: rotate(135deg);
  transform: rotate(135deg);
}

.check-item[notrequired] .check-label {
opacity: 0.4;
}

.check-item[notrequired] .checkmark {
  background-color: #eee;
  border-color: #eee;
}
.check-item[notrequired] .checkmark:after{
  content: "\2013";
  color: var(--v-naturalGrey-lighten4);
  left: 5px;
  top: -1px;
  -webkit-transform: none;
  -ms-transform: none;
  transform: none;
}


</style>
