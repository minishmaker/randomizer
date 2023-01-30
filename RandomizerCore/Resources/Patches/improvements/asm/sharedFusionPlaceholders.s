.thumb
ldrb	r4, [r0]
mov	r0, r4
cmp	r0, #0xF2
beq	placeholder

@ CheckKinstoneFused
ldr	r3, =#0x801E82C
mov	lr, r3
.short	0xF800

placeholder:
ldr	r3, =#0x801EA91
bx	r3
