const https = require('https')
const crypto = require('crypto')

const API_HOST = 'lingolette.com'
const API_VERSION = 1

class Lingolette {

  /**
   * Constructor
   *
   * @param {string} username
   * @param {string} apiSecret
   */
  constructor (username, apiSecret) {
    this.username = username
    this.apiSecret = apiSecret
  }

  /**
   * Make an API call
   *
   * @param {string} endpoint
   * @param {string} method
   * @param {Record<string, *>} payload
   * @returns {Promise<*>}
   */
  async call (endpoint, method, payload = undefined) {
    const
      random = crypto.randomBytes(32).toString('hex'),
      payloadString = JSON.stringify({method, data: payload}),
      hash = crypto.createHmac('sha256', this.apiSecret).update(random).digest('hex')

    const options = {
      method: 'POST',
      headers: {
        'Content-Length': payloadString.length,
        'Content-Type': 'application/json',
        'x-api-version': API_VERSION,
        'x-random': random,
        'x-auth-id': this.username,
        'x-auth-key': hash,
      },
    }

    return new Promise((resolve) => {
      const req = https.request(`https://${API_HOST}/api/${endpoint}`, options, res => {
        res.data = ''
        res.json = undefined

        res.on('data', chunk => res.data += chunk)
        res.on('error', e => resolve(e))
        res.on('end', () => resolve(JSON.parse(res.data)))
      })

      req.write(payloadString)
      req.end()
    })
  }
}

module.exports = { Lingolette }
