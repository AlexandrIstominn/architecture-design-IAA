(function () {
  const API_BASE = ''; // относительный путь — запросы идут через прокси на том же origin
  const getUserId = () => document.getElementById('userId').value || '1';

  function headers() {
    return {
      'Content-Type': 'application/json',
      'X-User-Id': getUserId(),
    };
  }

  function showMessage(elId, text, type) {
    const el = document.getElementById(elId);
    if (!el) return;
    el.textContent = text;
    el.className = 'message' + (type ? ' ' + type : '');
  }

  function setApiStatus(ok) {
    const el = document.getElementById('apiStatus');
    if (!el) return;
    el.textContent = ok ? 'подключён' : 'ошибка';
    el.className = ok ? 'ok' : 'fail';
  }

  async function api(path, options = {}) {
    const url = API_BASE + path;
    const res = await fetch(url, {
      ...options,
      headers: { ...headers(), ...(options.headers || {}) },
    });
    setApiStatus(res.ok || res.status === 404 || res.status === 204);
    if (res.status === 204) return null;
    const text = await res.text();
    if (!text) return null;
    try {
      return JSON.parse(text);
    } catch {
      return text;
    }
  }

  // ——— Запуск анализа ———
  document.getElementById('formAnalysis')?.addEventListener('submit', async (e) => {
    e.preventDefault();
    const form = e.target;
    const socialNetwork = form.socialNetwork.value.trim();
    const accountIds = form.accountIds.value.split(',').map(s => s.trim()).filter(Boolean);
    if (!accountIds.length) {
      showMessage('analysisMessage', 'Укажите хотя бы один аккаунт', 'error');
      return;
    }
    showMessage('analysisMessage', 'Отправка…');
    try {
      const data = await api('/api/analysis', {
        method: 'POST',
        body: JSON.stringify({ socialNetwork, accountIds }),
      });
      if (data && data.historyId != null) {
        showMessage('analysisMessage', `Анализ запущен, ID: ${data.historyId}`, 'success');
        form.accountIds.value = '';
        document.getElementById('btnRefreshAnalyses')?.click();
      } else {
        showMessage('analysisMessage', 'Не удалось запустить анализ', 'error');
      }
    } catch (err) {
      showMessage('analysisMessage', 'Ошибка: ' + err.message, 'error');
    }
  });

  // ——— Список анализов ———
  async function loadAnalyses() {
    const list = document.getElementById('analysesList');
    const detail = document.getElementById('analysisDetail');
    if (!list) return;
    list.innerHTML = 'Загрузка…';
    detail.innerHTML = '';
    try {
      const arr = await api('/api/analysis');
      if (!Array.isArray(arr)) {
        list.innerHTML = '<span class="message error">Нет данных</span>';
        return;
      }
      if (arr.length === 0) {
        list.innerHTML = '<span class="message">Нет анализов</span>';
        return;
      }
      list.innerHTML = arr
        .map(
          (a) =>
            `<div class="list-item" data-id="${a.historyId}">
              <span><strong>#${a.historyId}</strong> ${a.socialNetwork} — ${a.status}</span>
              <span class="meta">${new Date(a.analysisDate).toLocaleString('ru')}</span>
            </div>`
        )
        .join('');

      list.querySelectorAll('.list-item').forEach((el) => {
        el.addEventListener('click', async () => {
          const id = el.dataset.id;
          detail.innerHTML = 'Загрузка…';
          try {
            const summary = await api(`/api/analysis/${id}`);
            detail.innerHTML = `<pre>${JSON.stringify(summary, null, 2)}</pre>`;
          } catch (err) {
            detail.innerHTML = `<span class="message error">${err.message}</span>`;
          }
        });
      });
    } catch (err) {
      list.innerHTML = '<span class="message error">' + err.message + '</span>';
    }
  }

  document.getElementById('btnRefreshAnalyses')?.addEventListener('click', loadAnalyses);

  // ——— Аккаунты ———
  document.getElementById('formAccount')?.addEventListener('submit', async (e) => {
    e.preventDefault();
    const form = e.target;
    const username = form.username.value.trim();
    const socialNetwork = form.socialNetwork.value.trim();
    try {
      const data = await api('/api/accounts', {
        method: 'POST',
        body: JSON.stringify({ username, socialNetwork }),
      });
      if (data && data.accountId != null) {
        form.username.value = '';
        loadAccounts();
      }
    } catch (err) {
      console.error(err);
    }
  });

  async function loadAccounts() {
    const list = document.getElementById('accountsList');
    if (!list) return;
    list.innerHTML = 'Загрузка…';
    try {
      const analyses = await api('/api/analysis');
      const ids = new Set();
      if (Array.isArray(analyses)) {
        for (const a of analyses) {
          const summary = await api(`/api/analysis/${a.historyId}`);
          if (summary && Array.isArray(summary.results)) {
            summary.results.forEach((r) => ids.add(r.accountId));
          }
        }
      }
      if (ids.size === 0) {
        list.innerHTML = '<span class="message">Добавьте аккаунты через форму или запустите анализ</span>';
        return;
      }
      const accounts = [];
      for (const id of ids) {
        try {
          const acc = await api(`/api/accounts/${id}`);
          if (acc) accounts.push(acc);
        } catch (_) {}
      }
      list.innerHTML =
        accounts.length === 0
          ? '<span class="message">Нет аккаунтов</span>'
          : accounts
              .map(
                (a) =>
                  `<div class="list-item">#${a.accountId} ${a.username} (${a.socialNetwork}) — ${a.status}</div>`
              )
              .join('');
    } catch (err) {
      list.innerHTML = '<span class="message error">' + err.message + '</span>';
    }
  }

  // ——— Отчёты ———
  async function loadReports() {
    const list = document.getElementById('reportsList');
    if (!list) return;
    list.innerHTML = 'Загрузка…';
    try {
      const arr = await api('/api/reports');
      if (!Array.isArray(arr)) {
        list.innerHTML = '<span class="message">Нет данных</span>';
        return;
      }
      if (arr.length === 0) {
        list.innerHTML = '<span class="message">Нет отчётов</span>';
        return;
      }
      list.innerHTML = arr
        .map(
          (r) =>
            `<div class="list-item" data-id="${r.reportId}">
              <span>Отчёт #${r.reportId}</span>
              <span class="meta">${r.fileFormat} — ${new Date(r.reportDate).toLocaleString('ru')}</span>
            </div>`
        )
        .join('');
    } catch (err) {
      list.innerHTML = '<span class="message error">' + err.message + '</span>';
    }
  }

  document.getElementById('btnRefreshReports')?.addEventListener('click', loadReports);

  // Первая загрузка
  loadAnalyses();
  loadReports();
})();
