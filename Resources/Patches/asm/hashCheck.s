.thumb
push	{r0-r7}
@check hash against expected values
ldr	r0,=#0x811DBD5
ldr	r1,=#0xE00000E
mov	r4,#0
checkLoop:
ldrb	r2,[r0,r4]
ldrb	r3,[r1]
cmp	r2,r3
bne	erase
add	r4,#1
cmp	r4,#7
beq	end
sub	r1,#1
b	checkLoop

@erase the whole thing
erase:
ldr	r0,=#0xE000000
ldr	r1,=#0xE002000
mov	r2,#0xFF
eraseLoop:
strb	r2,[r0]
add	r0,#1
cmp	r0,r1
bne	eraseLoop

end:
pop	{r0-r7}
ldr	r3,=#0x8055CEC
mov	lr,r3
.short	0xF800
ldr	r3,=#0x807C8B4
mov	lr,r3
.short	0xF800
ldr	r3,=#0x80559FB
bx	r3
