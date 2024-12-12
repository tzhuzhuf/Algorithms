#include <iostream>

using namespace std;

class Node {
public:
	int data;
	Node* next;

	Node(int value) {
		data = value;
		next = nullptr;
	}
};

class Stack {
public:
	Node* top;

public:
	Stack() {
		top = nullptr;
	}

	void Push(int value) {
		Node* newNode = new Node(value);
		newNode->next = top;
		top = newNode;
	}

	void Peek() {
		cout << top->data << endl;
	}

	void Pop() {
		Node* temp = top;
		top = top->next;
		delete temp;
	}

	void Display() {
		Node* current = top;
		while (top != nullptr) {
			cout << current->data<<endl;
			current = current->next;
		}
	}
};

int main() {
	Stack stack;
	stack.Push(3);
	stack.Push(5);
	stack.Push(2);
	stack.Pop();

	stack.Peek();
	stack.Display();
	
}