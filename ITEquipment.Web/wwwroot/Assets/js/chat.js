"use strict";var tiplAppChat=function(){var e=function(e){var t=e.querySelector('[data-tipl-element="messages"]'),n=e.querySelector('[data-tipl-element="input"]');if(0!==n.value.length){var o,a=t.querySelector('[data-tipl-element="template-out"]'),l=t.querySelector('[data-tipl-element="template-in"]');(o=a.cloneNode(!0)).classList.remove("d-none"),o.querySelector('[data-tipl-element="message-text"]').innerText=n.value,n.value="",t.appendChild(o),t.scrollTop=t.scrollHeight,setTimeout((function(){(o=l.cloneNode(!0)).classList.remove("d-none"),o.querySelector('[data-tipl-element="message-text"]').innerText="Thank you for your awesome support!",t.appendChild(o),t.scrollTop=t.scrollHeight}),2e3)}};return{init:function(t){!function(t){t&&(tiplUtil.on(t,'[data-tipl-element="input"]',"keydown",(function(n){if(13==n.keyCode)return e(t),n.preventDefault(),!1})),tiplUtil.on(t,'[data-tipl-element="send"]',"click",(function(n){e(t)})))}(t)}}}();tiplUtil.onDOMContentLoaded((function(){tiplAppChat.init(document.querySelector("#tipl_chat_messenger")),tiplAppChat.init(document.querySelector("#tipl_drawer_chat_messenger"))}));