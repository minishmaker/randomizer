.thumb
ldr	r1,=#0x800A100
cmp	r2,r1
beq	end
ldrb	r1,[r2,#3]
ldr	r2,=#0x80A7410
mov	lr,r2
mov	r2,#0
.short	0xF800
end:
pop	{pc}
