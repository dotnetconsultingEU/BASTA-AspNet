import http from 'k6/http';
import {check, sleep} from 'k6';

export const options = {
  vus: 5,
  duration: '30s',
};

export default function () {
  const url = 'http://localhost:50008/api/UserManagement/allPlants';

  const params = {
    headers: {
      'x-api-key': '3qfOpxieG7',
    },
  };

  let result = http.get(url, params);
  check(result, { 'allPlants': (r) => r.status === 200 });
  // sleep(0.3);
}