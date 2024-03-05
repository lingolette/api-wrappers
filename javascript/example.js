const { Lingolette } = require('./lingolette')

const api = new Lingolette('YOUR_USERNAME', 'YOUR_SECRET')

api.call('org', 'createUserSession', {
  userId: 'YOUR_ORG_USER_ID',
})
  .then(result => console.log(result))
  .catch(error => console.log('API error', error))
