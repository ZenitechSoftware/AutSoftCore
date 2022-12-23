/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   hook.js                                            :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: mafaussu <mafaussu@student.42lyon.fr>      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2022/06/30 13:00:00 by mafaussu          #+#    #+#             */
/*   Updated: 2022/07/05 14:34:21 by mafaussu         ###   ########lyon.fr   */
/*                                                                            */
/* ************************************************************************** */

const nav_state_time_key = '__tsd_nav_creation';
let nav_prev_position;
let nav_direction;
let nav_position;
let nav_history;

function get_nav_position() {
  return nav_position;
}

function get_nav_state_time_key() {
  if (history.state)
    return history.state[nav_state_time_key];

  return nav_history[0].state[nav_state_time_key];
}

function init_nav() {
  nav_direction = "initial page";
  nav_position = 1;
  nav_history = [{
    url: document.location.href,
    state: {
      [nav_state_time_key]: new Date().getTime()
    }
  }
  ];
}

init_nav();

window.addEventListener('nav::tick', function (e) {

})

function nav_dispatch(e = null) {
  window.dispatchEvent(new CustomEvent('nav::tick', {
    detail: {
      nav_prev_position: nav_prev_position,
      nav_position: nav_position,
      nav_history: nav_history,
      nav_state: nav_history[nav_position - 1].state,
      nav_direction: nav_direction,
      popstate_event: e
    }
  }));
}

nav_dispatch();

function is_nav_forward_possible() {
  return (nav_history.length > 1 && nav_position < nav_history.length);
}

function is_nav_backward_possible() {
  return ((nav_position > 1) || ((nav_position === 1) && (nav_history.length < window.history.length)));
}

window.history.pushState = hook(window.history.pushState, function (cb, ...args) {

  if (args.length < 2 || typeof args[0] !== 'object'
    || args[2] === window.location.pathname
  )
    return cb(args);

  if (args[0] == null) {
    args[0] = {};
  }
  args[0][nav_state_time_key] = new Date().getTime()
  const value = cb(args);
  nav_history.push({ url: document.location.href, state: args[0] });
  nav_position = nav_history.length;
  nav_direction = "forward";
  nav_dispatch();
  return (value);
});

window.history.replaceState = hook(window.history.replaceState, function (cb, ...args) {
  if (args.length < 2 || typeof args[0] !== 'object'
    || args[2] === window.location.pathname)
    return cb(args);

  if (args[0] == null) {
    args[0] = {};
  }

  args[0][nav_state_time_key] = new Date().getTime()
  nav_history[nav_position - 1] = { url: args[2], state: args[0] };
  const value = cb(args);
  nav_direction = "redirection";
  nav_dispatch();
  return (value);
});

window.history.go = hook(window.history.go, function (cb, ...args) {
  nav_prev_position = nav_position;
  const value = cb(args);
  if (args.length < 1 || isNaN(args[0]))
    return value;
  args[0] = Math.round(args[0]);
  if (args[0] === 0)
    return value;
  if ((nav_position + args[0] >= 1)
    ||
    (nav_position + args[0] <= nav_history.length))
    nav_position += args[0];
  return (value);
});

window.addEventListener("popstate", function (e) {
  nav_prev_position = nav_position;
  nav_position = 0;
  if (typeof e.state == 'undefined'
    || e.state == null
    || typeof e.state[nav_state_time_key] == 'undefined'
    || e.state[nav_state_time_key] == null)
    nav_position = 1;
  else {
    let k = nav_history.length - 1;
    while (k >= 0) {
      if (nav_history[k].url === document.location.href
        && nav_history[k].state[nav_state_time_key] === e.state[nav_state_time_key]) {
        nav_position = Number(k) + 1;
        break;
      }
      k -= 1
    }
  }
  if (nav_position < 1) {
    nav_position = nav_history.length;
    nav_direction = "redirection";
    nav_dispatch(e);
  }
  else {
    nav_direction = nav_position < nav_prev_position ? 'backward' : 'forward';
    nav_dispatch(e);
  }
});
