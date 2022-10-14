import http from 'k6/http';
import {check, sleep} from 'k6';

export const options = {
  vus: 5,
  duration: '30s',
};

export default function () {
  const url = 'http://localhost:50008/api/UserManagement/User?unumber=U000004';

  const params = {
    headers: {
      'x-api-key': '3qfOpxieG7',
    },
  };

  let result = http.get(url, params);
  check(result, { 'singleUser': (r) => r.status === 200 });
  // sleep(0.3);
}