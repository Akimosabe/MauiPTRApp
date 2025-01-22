namespace MauiPTRApp.Messages;


// Определение сообщения для передачи информации о книге между компонентами
public record BookMessage(MauiPTRApp.Models.Book Value, bool IsUpdate);
