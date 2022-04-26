<template>
  <div>
    <BaseCard title="Card Title" subtitle="Card Subtitle" divider>
      <ul>
        <li>Card Contents</li>
        <li>The props for title, subtitle and divider are added to the BaseCard tag. All are optional.</li>
        <li>Divider only shows if a prop for title and/or subtitle are defined.</li>
        <li>This will automaticly be wrapped in v-card-text so no need to add that here. Just add your content.</li>
      </ul>
    </BaseCard>
    <BaseCard title="No divider & no subtitle">
      Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vel tincidunt sapien, eu tristique diam. Phasellus bibendum semper tincidunt. Donec ornare velit consectetur euismod blandit. Curabitur porttitor sapien non risus tristique condimentum. Sed laoreet, leo non condimentum sagittis, libero arcu sodales nulla, ut eleifend orci risus ut lectus.
    </BaseCard>
    <BaseCard title="No divider & no subtitle & no text"></BaseCard>
    <BaseCard title="No divider" subtitle="Card Subtitle">
      Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vel tincidunt sapien, eu tristique diam. Phasellus bibendum semper tincidunt. Donec ornare velit consectetur euismod blandit. Curabitur porttitor sapien non risus tristique condimentum. Sed laoreet, leo non condimentum sagittis, libero arcu sodales nulla, ut eleifend orci risus ut lectus.
    </BaseCard>
    <BaseCard>
      <div>No title or subtitle</div><br />
      Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vel tincidunt sapien, eu tristique diam. Phasellus bibendum semper tincidunt. Donec ornare velit consectetur euismod blandit. Curabitur porttitor sapien non risus tristique condimentum. Sed laoreet, leo non condimentum sagittis, libero arcu sodales nulla, ut eleifend orci risus ut lectus.
    </BaseCard>
    <BaseCard title="Primary color with dark mode" subtitle="Card Subtitle" divider color="primary" dark>
      Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vel tincidunt sapien, eu tristique diam. Phasellus bibendum semper tincidunt. Donec ornare velit consectetur euismod blandit. Curabitur porttitor sapien non risus tristique condimentum. Sed laoreet, leo non condimentum sagittis, libero arcu sodales nulla, ut eleifend orci risus ut lectus.
    </BaseCard>
    <BaseCard title="Color other" subtitle="Card Subtitle" color="warning">
      Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vel tincidunt sapien, eu tristique diam. Phasellus bibendum semper tincidunt. Donec ornare velit consectetur euismod blandit. Curabitur porttitor sapien non risus tristique condimentum. Sed laoreet, leo non condimentum sagittis, libero arcu sodales nulla, ut eleifend orci risus ut lectus.
    </BaseCard>
    <BaseCard title="Card Title" subtitle="Card Subtitle" divider dark head-class="secondary" body-class="white light">
          <ul>
            <li>Card Contents</li>
            <li>The props for title, subtitle and divider are added to the BaseCard tag. All are optional.</li>
            <li>Divider only shows if a prop for title and/or subtitle are defined.</li>
            <li>This will automaticly be wrapped in v-card-text so no need to add that here. Just add your content.</li>
          </ul>
        </BaseCard>
        <BaseCard title="No divider No Subtitle" head-class="sucess" body-class="naturalGrey lighten-2" divider>
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vel tincidunt sapien, eu tristique diam. Phasellus bibendum semper tincidunt. Donec ornare velit consectetur euismod blandit. Curabitur porttitor sapien non risus tristique condimentum. Sed laoreet, leo non condimentum sagittis, libero arcu sodales nulla, ut eleifend orci risus ut lectus.
        </BaseCard>
        <BaseCard head-class="primary" body-class="naturalGrey lighten-3">
          <div>No title nor subtitle (No Head)</div><br />
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vel tincidunt sapien, eu tristique diam. Phasellus bibendum semper tincidunt. Donec ornare velit consectetur euismod blandit. Curabitur porttitor sapien non risus tristique condimentum. Sed laoreet, leo non condimentum sagittis, libero arcu sodales nulla, ut eleifend orci risus ut lectus.
        </BaseCard>
  </div>
</template>

<script>
export default {
  name: 'DemoFormTemplate',
  data: () => ({

  }),
};
</script>
