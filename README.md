### API wrappers

![GitHub](https://img.shields.io/github/license/lingolette/api-wrappers)
![GitHub language count](https://img.shields.io/github/languages/count/lingolette/api-wrappers)
![GitHub issues](https://img.shields.io/github/issues/lingolette/api-wrappers)

A collection of API wrappers for Lingolette. <br/>

___

**Added:**
* JavaScript
* C#
* PHP

**To add:**
* TypeScript
* Java
* Python

---


# Lingolette API Manual

**Updated:** 2024-10-17  
**API Version Code:** 1  

---

## Table of Contents

- [Prerequisites](#prerequisites)
- [Data Exchange](#data-exchange)
- [API Collections](#api-collections)
  - [Organizations (`org`)](#organizations-org)
  - [Lessons (`org/lesson`)](#lessons-orglesson)
  - [Text Operations (`text`)](#text-operations-text)
  - [Chats (`chat`)](#chats-chat)
  - [Non-JSON I/O (`binary`)](#non-json-io-binary)
- [Appendix A: Types and Enums](#appendix-a-types-and-enums)
- [Appendix B: Supported Languages](#appendix-b-supported-languages)

---

## Prerequisites

### Authentication

Lingolette uses **SHA256 HMAC** authentication. Required HTTP headers:

- `x-random`: Random string (≥ 32 bytes)
- `x-auth-id`: Your internal identifier
- `x-auth-key`: HMAC hash using the above and your secret key

👉 Test your hash: [HMAC Generator](https://www.freeformatter.com/hmac-generator.html)

### Versioning

Use the `x-api-version` header to indicate the API version. This is **required**.

### API Wrappers

Sample wrappers available at: [GitHub - Lingolette API Wrappers](https://github.com/lingolette/api-wrappers)

---

## Data Exchange

- **Request:** HTTP `POST` with `application/json` content type.
- **Endpoint Format:** `https://lingolette.com/api/{collection}`

### Example Request

```json
{
  "method": "createUserSession",
  "data": {
    "userId": "ddd3c9c2-5797-4215-9390-362106f6442a"
  }
}
```

### Example Response

```json
{
  "isError": false,
  "errMsg": null,
  "data": {
    ...
  }
}
```

---

## API Collections

### Organizations (`org`)

Endpoint: `https://lingolette.com/api/org`

| Method | Description | Input | Output |
|--------|-------------|-------|--------|
| `listUsers` | List all users | — | Array of user objects |
| `getOverview` | Overview including progress | — | Org info, users, admins |
| `addUser` | Add a user | User data | User object |
| `removeUser` | Remove a user | `{ userId }` | `"ok"` |
| `createUserSession` | Create session | `{ userId }` | `{ token }` |

🔗 Frontend token usage: `https://lingolette.com/?token=SESSION_TOKEN`

---

### Lessons (`org/lesson`)

Endpoint: `https://lingolette.com/api/org/lesson`

| Method | Description | Input | Output |
|--------|-------------|-------|--------|
| `createEphemeral` | One-time lesson | Lesson config | `{ url }` |
| `create` | Create a lesson | Lesson config | `{ id }` |
| `update` | Update lesson | Updated fields | `"ok"` |
| `delete` | Delete lesson | `{ id }` | `"ok"` |
| `list` | List lessons | Pagination optional | Array of lessons |
| `getById` | Get lesson by ID | `{ id }` | Lesson object |

---

### Text Operations (`text`)

Endpoint: `https://lingolette.com/api/text`

| Method | Description | Input | Output |
|--------|-------------|-------|--------|
| `translate` | Translate text or word | Text and language codes | Translation, lemma, POS |
| `explain` | Explain text or word | Text and targetLng | Explanation |

---

### Chats (`chat`)

Endpoint: `https://lingolette.com/api/chat`

| Method | Description | Input | Output |
|--------|-------------|-------|--------|
| `load` | Load current chat | — | Chat object |
| `clear` | Clear chat | — | — |
| `voiceInput` | Send voice input | Base64 data + chat ID | Text response |

---

### Non-JSON I/O (`binary`)

Endpoint: `https://lingolette.com/api/binary`  
Uses **Server-Sent Events (SSE)** for streaming.

| Method | Description | Input | Output |
|--------|-------------|-------|--------|
| `startChat` | Initialize a chat | Metadata (time, articleId, etc.) | `SSE: [ChatCommand, ...]` |
| `postToChat` | Send message + voice/text response | Input text & voice flags | `SSE: [ChatCommand, ...]` |

---

## Appendix A: Types and Enums

### `LngLevel`

| Code | Description |
|------|-------------|
| 0 | Unset |
| 1 | A1 |
| 2 | A2 |
| 3 | B1 |
| 4 | B2 |
| 5 | C1 |
| 6 | C2 |
| 7 | None |

### `ChatCommand`

| Code | Description |
|------|-------------|
| 0 | Close connection |
| 1 | Incoming text |
| 2 | Incoming voice |
| 3 | Language change |
| 4 | Custom AI function |
| 5 | Error |
| 6 | Feedback |
| 7 | Set last message ID |
| 8 | Set chat description |

### `LessonMode`

| Code | Description |
|------|-------------|
| 0 | Discuss a text |
| 1 | Talk on a topic |

### `FeedbackMode`

| Code | Description |
|------|-------------|
| 0 | Disabled |
| 1 | Inline |
| 2 | Separate note |

### `FormalityLevel`

| Code | Description |
|------|-------------|
| 0 | Informal |
| 1 | Formal |

### `GrammarFeatures` (Bitmask)

| Bitmask | Description |
|---------|-------------|
| `0x001` | Present tense |
| `0x010` | Past tense |
| `0x100` | Future tense |

---

## Appendix B: Supported Languages

Refer to the [ISO 639-1 List](https://localizely.com/iso-639-1-list)

### Target Languages (`TLng`)

- Full Mode: `ca, de, en, es, fi, fr, he, it, ja, nb, ms, nl, pl, pt, ru, sv, tr, uk, zh`
- Test Mode: `ar, da, fa, el, ko, lt, ro, sr, th`

### Native Languages (`Lng`)

- Any ISO-639-1 code.
