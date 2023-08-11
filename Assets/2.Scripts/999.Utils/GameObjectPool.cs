using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : class
{
    //������ ���Լ��� �̹Ƿ� ������ �ٷ� ���� �ٷ� ���� �ϴ� ����
    //�׷��� ���⿡ �ȸ±⶧���� ť�� ���

    //QUEUE
    //������Ʈ���� �޸𸮻����� ������ ������ ��
    public delegate T CreateFunc();

    Queue<T> m_objectQueue = new Queue<T>();
    int m_count;
    CreateFunc m_func;

    public GameObjectPool(int count, CreateFunc func)
    {
        m_count = count;
        m_func = func;
        Allocate();
    }

    void Allocate() //Queue�� �ִ� ����
    {
        for (int i = 0; i < m_count; i++)
        {
            m_objectQueue.Enqueue(m_func());
        }// ��������Ʈ �̿��ؼ� ��ȯ�� T���� ť�� �־���
    }
    public T Get()// Dequeque
    {
        if (m_objectQueue.Count > 0)
            return m_objectQueue.Dequeue();
        else
        {
            return m_func();
        }//���ϴ� ī��Ʈ�� ���� ���� ��� ������༭ ����.
    }

    public void Set(T obj)
    {
        m_objectQueue.Enqueue(obj);
    }
}
