const { Lingolette } = require('./lingolette')

const api = new Lingolette('YOUR_ORG_ID', 'YOUR_ORG_SECRET');

(async () => {
  console.log('\n===\nCreate user session\n---')

  try {
    const result = await api.call('org', 'createUserSession', {
      userId: 'YOUR_ORG_USER_ID',
    })
    console.log('Session token:', result.data?.token)
  } catch (error) {
    console.log('createUserSession API error:', error)
  }

  let newUserId = null

  console.log('\n===\nAdd user\n---')
  try {
    const result = await api.call('org', 'addUser', {
      name: 'John Doe',
      targetLng: 'es',
      nativeLng: 'en',
      languageLevel: 0,
    })

    console.log('User added:', result)
    newUserId = result.data?.id
  } catch (error) {
    console.log('addUser API error:', error)
  }

  console.log('\n===\nList users\n---')

  try {
    const result = await api.call('org', 'listUsers')

    console.log('Org users:', result.data)
  } catch (error) {
    console.log('listUsers API error:', error)
  }

  if (!newUserId) {
    return
  }

  console.log('\n===\nRemove user\n---')

  try {
    const result = await api.call('org', 'removeUser', {
      userId: newUserId,
    })

    console.log('User removed:', result)
  } catch (error) {
    console.log('removeUser API error:', error)
  }
})()
